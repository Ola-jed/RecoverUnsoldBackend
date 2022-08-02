using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldApi.Services.Opinions;
using RecoverUnsoldApi.Services.Orders;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OpinionsController : ControllerBase
{
    private readonly IOpinionsService _opinionsService;
    private readonly IOrdersService _ordersService;

    public OpinionsController(IOpinionsService opinionsService, IOrdersService ordersService)
    {
        _opinionsService = opinionsService;
        _ordersService = ordersService;
    }

    [Authorize]
    [HttpGet("/api/Orders/{id:guid}/Opinions")]
    public async Task<UrlPage<OpinionReadDto>> GetAll([FromRoute] Guid id, [FromQuery] int page = 1,
        [FromQuery] int perPage = 10)
    {
        var urlPaginationParam = new UrlPaginationParameter(
            perPage, page, this.GetCleanUrl(), nameof(page), nameof(perPage)
        );

        return await _opinionsService.Get(id, urlPaginationParam);
    }

    [Authorize]
    [HttpGet("{id:guid}", Name = nameof(GetOpinion))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OpinionReadDto>> GetOpinion(Guid id)
    {
        var opinion = await _opinionsService.Get(id);
        return opinion == null ? NotFound() : Ok(opinion);
    }
    
    [Authorize(Roles = Roles.Customer)]
    [HttpPost("/api/Orders/{id:guid}/Opinions")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<OpinionReadDto>> Create(Guid id, OpinionCreateDto opinionCreateDto)
    {
        var isOpinionRequestValid = await _ordersService.IsRelativeToCustomer(id, this.GetUserId());
        if (!isOpinionRequestValid)
        {
            return Forbid();
        }

        var opinion = await _opinionsService.Publish(opinionCreateDto, id);
        return CreatedAtRoute(nameof(GetOpinion), new { id = opinion.Id }, opinion);
    }
    
    [Authorize(Roles = Roles.Customer)]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Update(Guid id, OpinionUpdateDto opinionUpdateDto)
    {
        var isAuthor = await _opinionsService.IsUserAuthor(this.GetUserId(), id);
        if (!isAuthor)
        {
            return Forbid();
        }
        
        await _opinionsService.Update(id, opinionUpdateDto);
        return NoContent();
    }
    
    [Authorize(Roles = Roles.Customer)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var isAuthor = await _opinionsService.IsUserAuthor(this.GetUserId(), id);
        if (!isAuthor)
        {
            return Forbid();
        }
        
        await _opinionsService.Delete(id);
        return NoContent();
    }
}