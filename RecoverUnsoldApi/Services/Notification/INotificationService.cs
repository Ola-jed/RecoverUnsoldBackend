using RecoverUnsoldApi.Entities;
using RecoverUnsoldApi.Services.Notification.NotificationMessage;

namespace RecoverUnsoldApi.Services.Notification;

public interface INotificationService
{
    Task Send(INotificationMessage notificationMessage, params User[] users);
    void BackgroundSend(INotificationMessage notificationMessage, params User[] users);
}