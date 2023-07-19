using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;
using RecoverUnsoldDomain.Entities.Enums;

namespace RecoverUnsoldApi.Services.Alerts;

public class AlertsService : IAlertsService
{
    private readonly DataContext _context;

    public AlertsService(DataContext context)
    {
        _context = context;
    }

    public async Task CreateAlertForAllOffers(Guid customerId)
    {
        if (await _context.Alerts.AnyAsync(
                a => a.CustomerId == customerId && a.AlertType == AlertType.AnyOfferPublished)) return;

        _context.Alerts.Add(new Alert
        {
            AlertType = AlertType.AnyOfferPublished,
            CustomerId = customerId
        });
        await _context.SaveChangesAsync();
    }

    public async Task CreateForDistributorOffers(Guid customerId, Guid distributorId)
    {
        if (await _context.Alerts.AnyAsync(a => a.CustomerId == customerId && a.Trigger == distributorId.ToString()))
            return;

        _context.Alerts.Add(new Alert
        {
            AlertType = AlertType.DistributorOfferPublished,
            Trigger = distributorId.ToString(),
            CustomerId = customerId
        });
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AlertReadDto>> GetAlerts(Guid customerId)
    {
        return await Task.Run(() => _context.Alerts
            .GroupJoin<Alert, Distributor, string, AlertReadDto>(_context.Distributors,
                alert => alert.Trigger, distributor => distributor.Id.ToString(),
                (alert, distributors) => new AlertReadDto(alert.Id, alert.AlertType,
                    distributors.FirstOrDefault().ToDistributorInformationDto())
            ));
    }

    public async Task<bool> IsOwnedByUser(Guid alertId, Guid customerId)
    {
        return await _context.Alerts.AnyAsync(a => a.Id == alertId && a.CustomerId == customerId);
    }

    public async Task DeleteAlert(Guid alertId)
    {
        await _context.Alerts
            .Where(a => a.Id == alertId)
            .ExecuteDeleteAsync();
    }
}