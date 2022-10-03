namespace RecoverUnsoldDomain.Services.Notification.NotificationMessage;

public class OrderMadeNotificationMessage : INotificationMessage
{
    private readonly DateTime _publishDate;

    public OrderMadeNotificationMessage(DateTime publishDate)
    {
        _publishDate = publishDate;
    }

    public string Title => "Nouvelle commande reçue";

    public string Body =>
        $"Une commande vient d'etre passée pour une offre que vous avez publiée le {_publishDate.ToShortDateString()}";
}