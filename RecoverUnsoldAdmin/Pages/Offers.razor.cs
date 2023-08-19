using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldAdmin.Services.AccountSuspensions;
using RecoverUnsoldAdmin.Services.Offers;
using RecoverUnsoldAdmin.Utils;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Pages;

public class OffersBase : ComponentBase
{
    protected bool Loading { get; private set; } = true;
    protected bool ShowFilter { get; set; }
    protected MudTable<Offer>? Table { get; set; }
    protected OffersFilter Filter { get; set; } = new();

    [Inject]
    public IOffersService OffersService { get; set; } = default!;

    [Inject]
    public IAccountSuspensionsService AccountSuspensionsService { get; set; } = default!;


    [Inject]
    protected IStringLocalizer<App> StringLocalizer { get; set; } = default!;

    protected async Task OnSearch()
    {
        ShowFilter = false;
        await Table!.ReloadServerData();
    }

    protected async Task ResetFilters()
    {
        ShowFilter = false;
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

    protected void ToggleActiveLabel(string value)
    {
        Filter.Active = value switch
        {
            ActiveLabels.Active => true,
            ActiveLabels.Expired => false,
            _ => null
        };
    }
}