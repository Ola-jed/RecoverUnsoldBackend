using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RecoverUnsoldAdmin.Services.Distributors;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Pages;

public class DistributorsBase : ComponentBase
{
    protected bool Loading { get; private set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    protected string? Search { get; set; }
    protected MudTable<Distributor>? Table { get; set; }
    protected HashSet<Guid> ExpandedRows { get; set; } = new();

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IDistributorsService DistributorsService { get; set; } = default!;

    protected void ToggleDistributor(Guid id)
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

    protected async Task<TableData<Distributor>> ServerData(TableState arg)
    {
        PageNumber = arg.Page + 1;
        PageSize = arg.PageSize;
        var paginationParam = new UrlPaginationParameter(PageSize, PageNumber, NavigationManager.Uri, nameof(PageNumber));
        var distributorsPage = await DistributorsService.ListDistributors(paginationParam, Search);
        Loading = false;
        return new TableData<Distributor> { Items = distributorsPage.Items, TotalItems = distributorsPage.Total };
    }
}