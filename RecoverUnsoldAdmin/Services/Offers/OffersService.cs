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

    public async Task<Page<Offer>> ListOffers(OffersFilter offersFilter)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        var paginationParameter = new PaginationParameter(offersFilter.PerPage, offersFilter.Page);
        return context
            .Offers
            .AsNoTracking()
            .Include(o => o.Products)
            .ApplyFilters(offersFilter)
            .Paginate(paginationParameter, o => o.CreatedAt, PaginationOrder.Descending);
    }
}