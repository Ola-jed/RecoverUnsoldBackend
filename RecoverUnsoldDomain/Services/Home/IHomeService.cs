using RecoverUnsoldDomain.Dto;

namespace RecoverUnsoldDomain.Services.Home;

public interface IHomeService
{
    Task<CustomerHomeDto> GetCustomerHomeInformation(Guid? customerId);

    Task<DistributorHomeDto> GetDistributorHomeInformation(Guid distributorId, DateTime periodStart, DateTime periodEnd);
}