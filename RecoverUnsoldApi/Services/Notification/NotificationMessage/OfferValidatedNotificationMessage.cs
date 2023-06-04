using RecoverUnsoldDomain.MessageBuilders;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Services.Notification.NotificationMessage;

public class OfferValidatedNotificationMessage : IFirebaseMessageBuilder
{
    private readonly List<string> _fcmTokens;

    public OfferValidatedNotificationMessage(List<string> fcmTokens)
    {
        _fcmTokens = fcmTokens;
    }

    public FirebaseMessage BuildFirebaseMessage()
    {
        return new FirebaseMessage
        {
            Title = "Offre validée avec succès",
            Body =
                "Vous venez de valider une offre sur notre application. Vous serez tenu au courant de l'avancée du traitement de votre commande",
            FcmTokens = _fcmTokens
        };
    }
}