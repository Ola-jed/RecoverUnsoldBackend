using System.Collections.Immutable;
using FirebaseAdmin.Messaging;
using RecoverUnsoldDomain.Entities;
using RecoverUnsoldDomain.Infrastructure;
using RecoverUnsoldDomain.Services.Notification.NotificationMessage;

namespace RecoverUnsoldDomain.Services.Notification;

public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;
    private readonly BackgroundWorkerQueue _backgroundWorkerQueue;

    public NotificationService(ILogger<NotificationService> logger,
        BackgroundWorkerQueue backgroundWorkerQueue)
    {
        _logger = logger;
        _backgroundWorkerQueue = backgroundWorkerQueue;
    }

    public async Task Send(INotificationMessage notificationMessage, params User[] users)
    {
        var messages = users.SelectMany(user => user.FcmTokens, (_, fcmToken) => new Message
        {
            Token = fcmToken.Value,
            Notification = new FirebaseAdmin.Messaging.Notification
            {
                Title = notificationMessage.Title,
                Body = notificationMessage.Body
            }
        }).ToArray();
        
        if (messages.Length == 0)
        {
            return;
        }
        
        try
        {
            var result = await FirebaseMessaging.DefaultInstance.SendAllAsync(messages);
            foreach (var resultResponse in result?.Responses ?? ImmutableList<SendResponse>.Empty)
            {
                if (resultResponse.IsSuccess)
                {
                    _logger.LogInformation("Successfully sent message to: {MessageId}", resultResponse.MessageId);
                }
                else
                {
                    _logger.LogError("Error sending message to: {MessageId}\n Error : {ErrorMessage}",
                        resultResponse.MessageId, resultResponse.Exception.Message);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Notification sending failed : {}", e.Message);
        }
    }

    public void BackgroundSend(INotificationMessage notificationMessage, params User[] users)
    {
        _backgroundWorkerQueue.QueueBackgroundWorkItem(async _ => await Send(notificationMessage, users));
    }
}