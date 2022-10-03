namespace RecoverUnsoldDomain.Services.Notification.NotificationMessage;

public class OfferPublishedNotificationMessage : INotificationMessage
{
    private readonly decimal _price;
    private readonly DateTime _startDate;
    
    public OfferPublishedNotificationMessage(decimal price, DateTime startDate)
    {
        _price = price;
        _startDate = startDate;
    }
    
    public string Title => "Nouvelle offre";
    public string Body => $"Une nouvelle offre d'un montant total de {_price} et débutant le {_startDate.ToShortDateString()} vient d\'être publiée";
}