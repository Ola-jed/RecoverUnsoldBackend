using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Distributors;

public interface IDistributorsService
{
    Task<UrlPage<DistributorInformationDto>> GetDistributors(UrlPaginationParameter urlPaginationParameter,
        string? name = null);
    Task<DistributorInformationDto?> GetDistributor(Guid id);
}