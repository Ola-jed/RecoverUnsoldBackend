using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Data;
using RecoverUnsoldApi.Entities;
using RecoverUnsoldApi.Entities.Enums;
using RecoverUnsoldApi.Infrastructure;
using RecoverUnsoldApi.Services.Mail;
using RecoverUnsoldApi.Services.Mail.Mailable;
using RecoverUnsoldApi.Services.Notification.NotificationMessage;

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
            var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
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

            if (usersToNotify.Length > 0)
            {
                var offerPublishedMail = new OfferPublishedMail(
                    offer.Price,
                    offer.StartDate,
                    usersToNotify.Select(c => c!.Email)
                );
                await mailService.SendEmailAsync(offerPublishedMail);

                var offerPublishedNotificationMessage =
                    new OfferPublishedNotificationMessage(offer.Price, offer.StartDate);
                await notificationService.Send(offerPublishedNotificationMessage, usersToNotify);
            }
        });
    }
}