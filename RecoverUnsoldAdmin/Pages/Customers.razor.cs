using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RecoverUnsoldAdmin.Services.Customers;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Pages;

public class CustomersBase : ComponentBase
{
    protected bool Loading { get; private set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    protected string? Search { get; set; }
    protected MudTable<Customer>? Table { get; set; }
    protected HashSet<Guid> ExpandedRows { get; set; } = new();

    [Inject]
    private ICustomersService CustomersService { get; set; } = default!;

    protected void ToggleCustomer(Guid id)
    {
        if (ExpandedRows.Contains(id))
        {
            ExpandedRows.Remove(id);
        }
        else
        {
            ExpandedRows.Add(id);
        }
    }

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