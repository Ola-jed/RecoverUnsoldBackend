using RecoverUnsoldDomain.Entities;
using RecoverUnsoldDomain.Services.Notification.NotificationMessage;

namespace RecoverUnsoldDomain.Services.Notification;

public interface INotificationService
{
    Task Send(INotificationMessage notificationMessage, params User[] users);
    void BackgroundSend(INotificationMessage notificationMessage, params User[] users);
}