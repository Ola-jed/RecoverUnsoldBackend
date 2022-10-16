using Microsoft.AspNetCore.Components;
using RecoverUnsoldAdmin.Services.Offers;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Pages;

public class OfferDetailsBase: ComponentBase
{
    [Parameter]
    public Guid OfferId { get; set; }
    [Inject]
    public IOffersService OffersService { get; set; } = default!;
    protected bool Loading { get; set; } = true;
    protected Offer? Offer { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        Offer = await OffersService.GetOffer(OfferId);
        Loading = false;
    }
}