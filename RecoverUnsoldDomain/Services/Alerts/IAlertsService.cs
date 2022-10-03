using RecoverUnsoldDomain.Dto;

namespace RecoverUnsoldDomain.Services.Alerts;

public interface IAlertsService
{
    Task CreateAlertForAllOffers(Guid customerId);
    Task CreateForDistributorOffers(Guid customerId, Guid distributorId);
    Task<IEnumerable<AlertReadDto>> GetAlerts(Guid customerId);
    Task<bool> IsOwnedByUser(Guid alertId, Guid customerId);
    Task DeleteAlert(Guid alertId);
}