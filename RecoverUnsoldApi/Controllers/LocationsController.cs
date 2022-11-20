using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldApi.Services.Locations;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Distributor)]
public class LocationsController : ControllerBase
{
    private readonly ILocationsService _locationsService;

    public LocationsController(ILocationsService locationsService)
    {
        _locationsService = locationsService;
    }

    [HttpGet]
    public async Task<Page<LocationReadDto>> GetAll([FromQuery] int page = 1, [FromQuery] int perPage = 10)
    {
        var paginationParam = new PaginationParameter(perPage, page);
        return await _locationsService.GetAll(this.GetUserId(), paginationParam);
    }

    [AllowAnonymous]
    [HttpGet("Distributors/{distributorId:guid}")]
    public async Task<Page<LocationReadDto>> GetDistributorLocations(Guid distributorId, [FromQuery] int page = 1,
        [FromQuery] int perPage = 10)
    {
        var paginationParam = new PaginationParameter(perPage, page);
        return await _locationsService.GetAll(distributorId, paginationParam);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = nameof(GetLocation))]
    [ProducesResponseType(typeof(LocationReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LocationReadDto>> GetLocation(Guid id)
    {
        var location = await _locationsService.Get(id);
        return location == null ? NotFound() : location;
    }

    [AllowAnonymous]
    [HttpGet("Search/{query}")]
    public async Task<Page<LocationReadDto>> Search(string query, [FromQuery] int page = 1, [FromQuery] int perPage = 10)
    {
        var paginationParam = new PaginationParameter(perPage, page);
        return await _locationsService.FindByName(query, paginationParam);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<LocationReadDto>> Create([FromForm] LocationCreateDto locationCreateDto)
    {
        var userId = this.GetUserId();
        var location = await _locationsService.Create(userId, locationCreateDto);
        return CreatedAtRoute(nameof(GetLocation), new { id = location.Id }, location);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(Guid id, [FromForm] LocationUpdateDto locationUpdateDto)
    {
        var userId = this.GetUserId();
        var isOwned = await _locationsService.IsOwner(userId, id);
        if (!isOwned)
        {
            return NotFound();
        }

        await _locationsService.Update(userId, id, locationUpdateDto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var userId = this.GetUserId();
        var isOwned = await _locationsService.IsOwner(userId, id);
        if (!isOwned)
        {
            return NotFound();
        }

        await _locationsService.Delete(userId, id);
        return NoContent();
    }
}