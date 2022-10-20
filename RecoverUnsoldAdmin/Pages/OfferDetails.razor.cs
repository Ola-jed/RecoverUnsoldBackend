using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RecoverUnsoldAdmin.Services.Offers;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Pages;

public class OfferDetailsBase : ComponentBase
{
    [Parameter]
    public Guid OfferId { get; set; }

    [Inject]
    public IOffersService OffersService { get; set; } = default!;

    [Inject]
    protected IStringLocalizer<App> StringLocalizer { get; set; } = default!;

    protected bool Loading { get; private set; } = true;
    protected Offer? Offer { get; private set; }
    protected GoogleMap? OfferMap { get; set; }
    protected MapOptions MapOptions { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Offer = await OffersService.GetOffer(OfferId);
        if (Offer != null)
        {
            MapOptions = new MapOptions
            {
                Zoom = 13,
                Center = new LatLngLiteral
                {
                    Lat = Offer!.Location!.Coordinates.Y,
                    Lng = Offer!.Location!.Coordinates.X
                },
                MapTypeId = MapTypeId.Hybrid
            };
        }

        Loading = false;
    }

    protected async Task InitMarker()
    {
        if (OfferMap != null)
        {
            await Marker.CreateAsync(OfferMap.JsRuntime, new MarkerOptions
            {
                Clickable = true,
                Position = new LatLngLiteral
                {
                    Lat = Offer!.Location!.Coordinates.Y,
                    Lng = Offer!.Location!.Coordinates.X
                },
                Map = OfferMap.InteropObject
            });
        }
    }
}