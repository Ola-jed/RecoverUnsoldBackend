using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Services.Distributors;

public class DistributorsService: IDistributorsService
{
    private readonly IDbContextFactory<DataContext> _dbContextFactory;

    public DistributorsService(IDbContextFactory<DataContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<UrlPage<Distributor>> ListDistributors(UrlPaginationParameter urlPaginationParameter, string? name = null)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        var distributorsSource = context
            .Distributors
            .Include(d => d.Locations)
            .AsNoTracking();
        if(name != null && name.Trim() != string.Empty)
        {
            distributorsSource = distributorsSource.Where(d => EF.Functions.Like(d.Username, $"%{name}%"));
        }
        var distributors = distributorsSource
            .UrlPaginate(urlPaginationParameter, o => o.CreatedAt, PaginationOrder.Descending);
        return distributors;
    }
}