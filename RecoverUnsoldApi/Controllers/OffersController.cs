using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldApi.Services.Locations;
using RecoverUnsoldApi.Services.Notification.OfferPublishedNotification;
using RecoverUnsoldApi.Services.Offers;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OffersController : ControllerBase
{
    private readonly IOffersService _offersService;
    private readonly ILocationsService _locationsService;
    private readonly IOfferPublishedNotificationService _offerPublishedNotificationService;
    
    public OffersController(IOffersService offersService, ILocationsService locationsService,
        IOfferPublishedNotificationService offerPublishedNotificationService)
    {
        _offersService = offersService;
        _locationsService = locationsService;
        _offerPublishedNotificationService = offerPublishedNotificationService;
    }

    [HttpGet("{id:guid}", Name = nameof(GetOffer))]
    [ProducesResponseType(typeof(OfferReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OfferReadDto>> GetOffer(Guid id)
    {
        var offer = await _offersService.GetOffer(id);
        return offer == null ? NotFound() : offer;
    }

    [HttpGet]
    public async Task<UrlPage<OfferReadDto>> GetOffers([FromQuery] OfferFilterDto filterDto)
    {
        var urlPaginationParam = new UrlPaginationParameter(
            filterDto.PerPage, filterDto.Page, this.GetCleanUrl(), nameof(filterDto.Page), nameof(filterDto.PerPage)
        );
        return await _offersService.GetOffers(urlPaginationParam, filterDto);
    }

    [HttpGet("Distributors/{id:guid}")]
    public async Task<UrlPage<OfferReadDto>> GetDistributorOffers(Guid id, [FromQuery] OfferFilterDto filterDto)
    {
        var urlPaginationParam = new UrlPaginationParameter(
            filterDto.PerPage, filterDto.Page, this.GetCleanUrl(), nameof(filterDto.Page), nameof(filterDto.PerPage)
        );
        return await _offersService.GetDistributorOffers(id, urlPaginationParam, filterDto);
    }

    [HttpGet("CloseToLocation")]
    public async Task<UrlPage<OfferWithRelativeDistanceDto>> GetOffersCloseTo(
        [FromQuery] OfferDistanceFilterDto distanceFilterDto)
    {
        var urlPaginationParam = new UrlPaginationParameter(
            distanceFilterDto.PerPage, distanceFilterDto.Page, this.GetCleanUrl(),
            nameof(distanceFilterDto.Page), nameof(distanceFilterDto.PerPage)
        );
        return await _offersService.GetOffersCloseTo(
            distanceFilterDto.ToLatLong(),
            urlPaginationParam,
            distanceFilterDto.Distance);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Distributor)]
    [ProducesResponseType(typeof(OfferReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(OfferReadDto), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OfferReadDto>> Create([FromForm] OfferCreateDto offerCreateDto)
    {
        var distributorId = this.GetUserId();
        var isLocationOwnedByCurrentDistributor = await _locationsService.IsOwner(
            distributorId,
            offerCreateDto.LocationId
        );
        if (!isLocationOwnedByCurrentDistributor)
        {
            return BadRequest();
        }
        var offer = await _offersService.Create(distributorId, offerCreateDto);
        _offerPublishedNotificationService.Process(offer.Id);
        return CreatedAtRoute(nameof(GetOffer), new { id = offer.Id }, offer);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Distributor)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] OfferUpdateDto offerUpdateDto)
    {
        var distributorId = this.GetUserId();
        var isOwner = await _offersService.IsOwner(distributorId, id);
        if (!isOwner)
        {
            return NotFound();
        }

        await _offersService.Update(id, distributorId, offerUpdateDto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Distributor)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var distributorId = this.GetUserId();
        var isOwner = await _offersService.IsOwner(distributorId, id);
        if (!isOwner)
        {
            return NotFound();
        }

        await _offersService.Delete(id, distributorId);
        return NoContent();
    }
}