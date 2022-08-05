using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Home;

public interface IHomeService
{
    Task<CustomerHomeDto> GetCustomerHomeInformation();
}