using RecoverUnsoldDomain.MessageBuilders;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Notification.NotificationMessage;

public class OrderMadeNotificationMessage : IFirebaseMessageBuilder
{
    private readonly List<string> _fcmTokens;
    private readonly DateTime _publishDate;

    public OrderMadeNotificationMessage(DateTime publishDate, List<string> fcmTokens)
    {
        _publishDate = publishDate;
        _fcmTokens = fcmTokens;
    }

    public FirebaseMessage BuildFirebaseMessage()
    {
        return new FirebaseMessage
        {
            Title = "Nouvelle commande reçue",
            Body =
                $"Une commande vient d'etre passée pour une offre que vous avez publiée le {_publishDate.ToShortDateString()}",
            FcmTokens = _fcmTokens
        };
    }
}