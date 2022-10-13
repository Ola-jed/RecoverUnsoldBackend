using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Services.Customers;

public class CustomersService : ICustomersService
{
    private readonly IDbContextFactory<DataContext> _dbContextFactory;

    public CustomersService(IDbContextFactory<DataContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Page<Customer>> ListCustomers(PaginationParameter paginationParameter,
        string? name = null)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        var customersSource = context
            .Customers
            .AsNoTracking();
        if (name != null && name.Trim() != string.Empty)
        {
            customersSource = customersSource.Where(d =>
                EF.Functions.Like(d.Username, $"%{name}%")
                || EF.Functions.Like(d.FirstName!, $"%{name}%")
                || EF.Functions.Like(d.LastName!, $"%{name}%"));
        }

        return customersSource.Paginate(paginationParameter, o => o.CreatedAt, PaginationOrder.Descending);
    }
}