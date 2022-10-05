using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Dto;
using RecoverUnsoldDomain.Entities;
using RecoverUnsoldDomain.Entities.Enums;
using RecoverUnsoldDomain.Extensions;

namespace RecoverUnsoldDomain.Services.Alerts;

public class AlertsService : IAlertsService
{
    private readonly DataContext _context;

    public AlertsService(DataContext context)
    {
        _context = context;
    }

    public async Task CreateAlertForAllOffers(Guid customerId)
    {
        if (await _context.Alerts.AnyAsync(a => a.CustomerId == customerId && a.AlertType == AlertType.AnyOfferPublished))
        {
            return;
        }
        
        _context.Alerts.Add(new Alert
        {
            AlertType = AlertType.AnyOfferPublished,
            CustomerId = customerId
        });
        await _context.SaveChangesAsync();
    }

    public async Task CreateForDistributorOffers(Guid customerId, Guid distributorId)
    {
        if (await _context.Alerts.AnyAsync(
                a => a.CustomerId == customerId && a.Trigger == distributorId.ToString()))
        {
            return;
        }
        
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
        var alerts = await _context.Alerts
            .AsNoTracking()
            .Where(a => a.CustomerId == customerId)
            .ToListAsync();

        var distributors = alerts.Where(a => a.Trigger != null)
            .AsEnumerable()
            .Select(a => Guid.Parse(a.Trigger!))
            .Distinct()
            .Select(id => _context.Distributors.Find(id)?.ToDistributorInformationDto())
            .Where(d => d != null);

        return alerts.Select(a => new AlertReadDto
        (
            a.Id,
            a.AlertType,
            a.Trigger == null ? null : distributors.First(d => d!.Id == Guid.Parse(a.Trigger))
        ));
    }

    public async Task<bool> IsOwnedByUser(Guid alertId, Guid customerId)
    {
        return await _context.Alerts.AnyAsync(a => a.Id == alertId && a.CustomerId == customerId);
    }

    public async Task DeleteAlert(Guid alertId)
    {
        var alert = await _context.Alerts.FindAsync(alertId);
        if (alert == null)
        {
            return;
        }

        _context.Alerts.Remove(alert);
        await _context.SaveChangesAsync();
    }
}