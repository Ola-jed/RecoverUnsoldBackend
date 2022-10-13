using Microsoft.AspNetCore.Components;
using MudBlazor;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldAdmin.Services.Offers;
using RecoverUnsoldAdmin.Utils;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Pages;

public class OffersBase : ComponentBase
{
    protected bool Loading { get; private set; } = true;
    protected bool ShowFilter { get; set; }
    protected MudTable<Offer>? Table { get; set; }
    protected HashSet<Guid> ExpandedRows { get; set; } = new();
    protected OffersFilter Filter { get; set; } = new();

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IOffersService OffersService { get; set; } = default!;

    protected async Task OnSearch()
    {
        await Table!.ReloadServerData();
    }

    protected async Task ResetFilters()
    {
        Filter = new OffersFilter();
        await Table!.ReloadServerData();
    }

    protected async Task<TableData<Offer>> ServerData(TableState arg)
    {
        Filter.Page = arg.Page + 1;
        Filter.PerPage = arg.PageSize;
        var distributorsPage = await OffersService.ListOffers(Filter);
        Loading = false;
        return new TableData<Offer> { Items = distributorsPage.Items, TotalItems = distributorsPage.Total };
    }

    protected void ToggleOffer(Guid id)
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

    protected void ToggleActiveLabel(string value)
    {
        Filter.Active = value switch
        {
            ActiveLabels.Active  => true,
            ActiveLabels.Expired => false,
            _                    => null
        };
    }
}