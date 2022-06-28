using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldApi.Services.Offers;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OffersController : ControllerBase
{
    private readonly IOffersService _offersService;

    public OffersController(IOffersService offersService)
    {
        _offersService = offersService;
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

    [HttpPost]
    [Authorize(Roles = Roles.Distributor)]
    [ProducesResponseType(typeof(OfferReadDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<OfferReadDto>> Create([FromForm] OfferCreateDto offerCreateDto)
    {
        var distributorId = this.GetUserId();
        var offer = await _offersService.Create(distributorId, offerCreateDto);
        return CreatedAtRoute(nameof(GetOffer), new { id = offer.Id }, offer);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Distributor)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(Guid id, OfferUpdateDto offerUpdateDto)
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