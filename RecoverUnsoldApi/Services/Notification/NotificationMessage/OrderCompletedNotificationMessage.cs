namespace RecoverUnsoldApi.Services.Notification.NotificationMessage;

public class OrderCompletedNotificationMessage : INotificationMessage
{
    private readonly DateTime _orderDate;
    private readonly decimal _offerAmount;
    private readonly DateTime _offerPublishDate;
    
    public OrderCompletedNotificationMessage(DateTime orderDate, decimal offerAmount, DateTime offerPublishDate)
    {
        _orderDate = orderDate;
        _offerAmount = offerAmount;
        _offerPublishDate = offerPublishDate;
    }
    
    public string Title => "Commande finalisée";

    public string Body =>
        $"La commande que vous avez passée le {_orderDate.ToShortDateString()}, relative à une offre d'un montant total de {_offerAmount} et publiée le {_offerPublishDate.ToShortDateString()} vient d'être finalisée par le distributeur.";
}