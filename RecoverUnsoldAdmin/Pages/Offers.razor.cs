using Microsoft.AspNetCore.Components;
using MudBlazor;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldAdmin.Services.Offers;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Pages;

public class OffersBase : ComponentBase
{
    protected bool Loading { get; private set; } = true;
    protected MudTable<Offer>? Table { get; set; }
    protected HashSet<Guid> ExpandedRows { get; set; } = new();
    protected OffersFilter Filter { get; set; } = new();

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IOffersService OffersService { get; set; } = default!;
}