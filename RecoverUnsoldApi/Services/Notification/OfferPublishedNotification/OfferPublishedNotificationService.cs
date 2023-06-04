using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Infrastructure;
using RecoverUnsoldApi.Services.Mail.Mailable;
using RecoverUnsoldApi.Services.Notification.NotificationMessage;
using RecoverUnsoldApi.Services.Queue;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;
using RecoverUnsoldDomain.Entities.Enums;

namespace RecoverUnsoldApi.Services.Notification.OfferPublishedNotification;

public class OfferPublishedNotificationService : IOfferPublishedNotificationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly BackgroundWorkerQueue _backgroundWorkerQueue;

    public OfferPublishedNotificationService(IServiceProvider serviceProvider,
        BackgroundWorkerQueue backgroundWorkerQueue)
    {
        _serviceProvider = serviceProvider;
        _backgroundWorkerQueue = backgroundWorkerQueue;
    }

    public void Process(Guid offerId)
    {
        _backgroundWorkerQueue.QueueBackgroundWorkItem(async (cancellationToken) =>
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var queueService = scope.ServiceProvider.GetRequiredService<IQueueService>();
            var offer = await context.Offers.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

            if (offer == null)
            {
                return;
            }

            var offerDistributorId = offer.DistributorId.ToString();
            var usersToNotify = (await context.Alerts
                    .AsNoTracking()
                    .Where(a => a.AlertType == AlertType.AnyOfferPublished || a.Trigger == offerDistributorId)
                    .Include(a => a.Customer)
                    .ThenInclude(c => c!.FcmTokens)
                    .Select(a => a.Customer)
                    .Distinct()
                    .ToArrayAsync(cancellationToken))
                .Cast<User>()
                .ToArray();

            var mailMessages = usersToNotify.Select(u =>
                new OfferPublishedMail(offer.Price, offer.StartDate, u.Email).BuildMailMessage());
            queueService.QueueMails(mailMessages);


            if (usersToNotify.Length > 0)
            {
                var fcmTokens = usersToNotify.SelectMany(u => u.FcmTokens)
                    .Select(f => f.Value);
                var offerPublishedNotificationMessage =
                    new OfferPublishedNotificationMessage(offer.Price, offer.StartDate, fcmTokens.ToList());
                queueService.QueueFirebaseMessage(offerPublishedNotificationMessage.BuildFirebaseMessage());
            }
        });
    }
}