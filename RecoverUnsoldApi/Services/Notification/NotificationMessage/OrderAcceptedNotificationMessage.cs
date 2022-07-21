namespace RecoverUnsoldApi.Services.Notification.NotificationMessage;

public class OrderAcceptedNotificationMessage: INotificationMessage
{
    private readonly DateTime _orderDate;
    private readonly decimal _offerAmount;
    private readonly DateTime _offerPublishDate;

    public OrderAcceptedNotificationMessage(DateTime orderDate, decimal offerAmount, DateTime offerPublishDate)
    {
        _orderDate = orderDate;
        _offerAmount = offerAmount;
        _offerPublishDate = offerPublishDate;
    }


    public string Title => "Commande acceptée";

    public string Body =>
        $"La commande que vous avez passée le {_orderDate.ToShortDateString()}, relative à une offre d'un montant total de {_offerAmount} et publiée le {_offerPublishDate.ToShortDateString()} vient d'être acceptée par le distributeur.";
}