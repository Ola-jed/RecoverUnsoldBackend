using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Home;

public interface IHomeService
{
    Task<CustomerHomeDto> GetCustomerHomeInformation(Guid? customerId);

    Task<DistributorHomeDto> GetDistributorHomeInformation(Guid distributorId, DateTime periodStart, DateTime periodEnd);
}