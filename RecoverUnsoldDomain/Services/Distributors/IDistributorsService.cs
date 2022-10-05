using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldDomain.Dto;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldDomain.Services.Distributors;

public interface IDistributorsService
{
    Task<UrlPage<DistributorInformationDto>> GetDistributors(UrlPaginationParameter urlPaginationParameter,
        string? name = null);
    Task<UrlPage<Distributor>> ListDistributors(UrlPaginationParameter urlPaginationParameter,
        string? name = null);
    Task<IEnumerable<DistributorLabelReadDto>> GetDistributorsLabels();
    Task<DistributorInformationDto?> GetDistributor(Guid id);
}