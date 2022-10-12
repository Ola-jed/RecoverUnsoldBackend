using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;

namespace RecoverUnsoldAdmin.Services.Distributors;

public interface IDistributorsService
{
    Task<Page<RecoverUnsoldDomain.Entities.Distributor>> ListDistributors(PaginationParameter paginationParameter,
        string? name = null);
}