using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using RecoverUnsoldAdmin.Services.Customers;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Pages;

public class CustomersBase : ComponentBase
{
    [Inject]
    protected IStringLocalizer<App> StringLocalizer { get; set; } = default!;

    [Inject]
    private ICustomersService CustomersService { get; set; } = default!;

    protected bool Loading { get; private set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    protected string? Search { get; set; }
    protected MudTable<Customer>? Table { get; set; }

    protected async Task OnSearch()
    {
        await Table!.ReloadServerData();
    }

    protected async Task<TableData<Customer>> ServerData(TableState arg)
    {
        PageNumber = arg.Page + 1;
        PageSize = arg.PageSize;
        var paginationParam = new PaginationParameter(PageSize, PageNumber);
        var distributorsPage = await CustomersService.ListCustomers(paginationParam, Search);
        Loading = false;
        return new TableData<Customer> { Items = distributorsPage.Items, TotalItems = distributorsPage.Total };
    }
}