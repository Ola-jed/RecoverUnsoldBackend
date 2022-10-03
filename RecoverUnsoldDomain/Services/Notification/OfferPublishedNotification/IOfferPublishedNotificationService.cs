namespace RecoverUnsoldDomain.Services.Notification.OfferPublishedNotification;

public interface IOfferPublishedNotificationService
{
    void Process(Guid offerId);
}