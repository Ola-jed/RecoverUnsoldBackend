using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldAdmin.Extensions;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Services.Offers;

public class OffersService: IOffersService
{
    private readonly IDbContextFactory<DataContext> _dbContextFactory;
    
    public OffersService(IDbContextFactory<DataContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<UrlPage<Offer>> ListOffers(OffersFilter offersFilter, UrlPaginationParameter paginationParameter)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();

        return context
            .Offers
            .AsNoTracking()
            .Include(o => o.Products)
            .ApplyFilters(offersFilter)
            .UrlPaginate(paginationParameter, o => o.CreatedAt, PaginationOrder.Descending);
    }
}