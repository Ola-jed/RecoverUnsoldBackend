using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Services.Customers;

public interface ICustomersService
{
    Task<Page<Customer>> ListCustomers(PaginationParameter paginationParameter, string? name = null);
}