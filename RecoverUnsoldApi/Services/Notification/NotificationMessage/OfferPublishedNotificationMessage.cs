using RecoverUnsoldDomain.MessageBuilders;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Notification.NotificationMessage;

public class OfferPublishedNotificationMessage : IFirebaseMessageBuilder
{
    private readonly List<string> _fcmTokens;
    private readonly decimal _price;
    private readonly DateTime _startDate;

    public OfferPublishedNotificationMessage(decimal price, DateTime startDate, List<string> fcmTokens)
    {
        _price = price;
        _startDate = startDate;
        _fcmTokens = fcmTokens;
    }

    public FirebaseMessage BuildFirebaseMessage()
    {
        return new FirebaseMessage
        {
            Title = "Nouvelle offre",
            Body =
                $"Une nouvelle offre d'un montant total de {_price} et débutant le {_startDate.ToShortDateString()} vient d\'être publiée",
            FcmTokens = _fcmTokens
        };
    }
}