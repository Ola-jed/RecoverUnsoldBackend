using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Distributors;

public interface IDistributorsService
{
    Task<Page<DistributorInformationDto>> GetDistributors(PaginationParameter paginationParameter, string? name = null);
    Task<IEnumerable<DistributorLabelReadDto>> GetDistributorsLabels();
    Task<DistributorInformationDto?> GetDistributor(Guid id);
}