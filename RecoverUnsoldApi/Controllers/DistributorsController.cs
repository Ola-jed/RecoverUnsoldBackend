using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.Distributors;

[ApiController]
[Route("api/[controller]")]
public class DistributorsController : ControllerBase
{
    private readonly IDistributorsService _distributorsService;

    public DistributorsController(IDistributorsService distributorsService)
    {
        _distributorsService = distributorsService;
    }

    [HttpGet]
    public async Task<UrlPage<DistributorInformationDto>> GetDistributors([FromQuery] int page = 1, [FromQuery] int perPage = 10)
    {
        var urlPaginationParam = new UrlPaginationParameter(
            perPage, page, this.GetCleanUrl(), nameof(page), nameof(perPage)
        );
        return await _distributorsService.GetDistributors(urlPaginationParam);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DistributorInformationDto>> GetDistributor(Guid id)
    {
        var distributor = await _distributorsService.GetDistributor(id);
        return distributor == null ? NotFound() : Ok(distributor);
    }
}