using FirebaseAdmin.Messaging;
using RecoverUnsoldApi.Entities;
using RecoverUnsoldApi.Infrastructure;
using RecoverUnsoldApi.Services.Notification.NotificationMessage;

namespace RecoverUnsoldApi.Services.Notification;

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
        });

        try
        {
            await FirebaseMessaging.DefaultInstance.SendAllAsync(messages);
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