using RecoverUnsoldApi.Entities;

namespace RecoverUnsoldApi.Services.Notification;

public interface INotificationService
{
    Task Send(string title, string body, params User[] users);
    void BackgroundSend(string title, string body, params User[] users);
}