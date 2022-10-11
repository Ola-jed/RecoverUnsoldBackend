using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Services.Customers;

public interface ICustomersService
{
    Task<UrlPage<Customer>> ListCustomers(UrlPaginationParameter urlPaginationParameter, string? name = null);
}