using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Home;

public interface IHomeService
{
    Task<CustomerHomeDto> GetCustomerHomeInformation();

    Task<DistributorHomeDto> GetDistributorHomeInformation(Guid distributorId, DateTime periodStart, DateTime periodEnd);
}