using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;

namespace RecoverUnsoldAdmin.Services.Distributors;

public interface IDistributorsService
{
    Task<UrlPage<RecoverUnsoldDomain.Entities.Distributor>> ListDistributors(UrlPaginationParameter urlPaginationParameter,
        string? name = null);
}