using RecoverUnsoldDomain.MessageBuilders;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Notification.NotificationMessage;

public class OrderRejectedNotificationMessage : IFirebaseMessageBuilder
{
    private readonly List<string> _fcmTokens;
    private readonly decimal _offerAmount;
    private readonly DateTime _offerPublishDate;
    private readonly DateTime _orderDate;

    public OrderRejectedNotificationMessage(DateTime orderDate, decimal offerAmount, DateTime offerPublishDate,
        List<string> fcmTokens)
    {
        _orderDate = orderDate;
        _offerAmount = offerAmount;
        _offerPublishDate = offerPublishDate;
        _fcmTokens = fcmTokens;
    }

    public FirebaseMessage BuildFirebaseMessage()
    {
        return new FirebaseMessage
        {
            Title = "Commande refusée",
            Body =
                $"La commande que vous avez passée le {_orderDate.ToShortDateString()}, relative à une offre d'un montant total de {_offerAmount} et publiée le {_offerPublishDate.ToShortDateString()} vient d'être refusée par le distributeur.",
            FcmTokens = _fcmTokens
        };
    }
}