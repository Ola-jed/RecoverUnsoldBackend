namespace RecoverUnsoldDomain.Services.Notification.NotificationMessage;

public class OfferValidatedNotificationMessage : INotificationMessage
{
    public string Title => "Offre validée avec succès";
    public string Body => "Vous venez de valider une offre sur notre application. Vous serez tenu au courant de l'avancée du traitement de votre commande";
}