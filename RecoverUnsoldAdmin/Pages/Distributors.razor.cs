using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RecoverUnsoldDomain.Entities;
using RecoverUnsoldDomain.Services.Distributors;

namespace RecoverUnsoldAdmin.Pages;

public class DistributorsBase : ComponentBase
{
    protected bool Loading { get; private set; } = true;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    protected string? Search { get; set; }
    protected MudTable<Distributor> Table { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IDistributorsService DistributorsService { get; set; } = default!;

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        Table.RowsPerPage = 10;
        return base.OnAfterRenderAsync(firstRender);
    }

    protected async Task OnSearch()
    {
        await Table.ReloadServerData();
    }
    
    protected async Task<TableData<Distributor>> ServerData(TableState arg)
    {
        Page = arg.Page + 1;
        PageSize = arg.PageSize;
        var urlPaginationParameter = new UrlPaginationParameter(
            PageSize,
            Page,
            NavigationManager.Uri,
            nameof(Page),
            nameof(PageSize)
        );
        var distributorsPage = await DistributorsService.ListDistributors(urlPaginationParameter, Search);
        Loading = false;
        return new TableData<Distributor>
        {
            Items = distributorsPage.Items,
            TotalItems = distributorsPage.Total
        };
    }
}