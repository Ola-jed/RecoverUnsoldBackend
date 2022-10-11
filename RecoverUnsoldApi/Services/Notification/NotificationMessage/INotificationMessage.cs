namespace RecoverUnsoldApi.Services.Notification.NotificationMessage;

public interface INotificationMessage
{
    public string Title { get; }
    public string Body { get; }
}