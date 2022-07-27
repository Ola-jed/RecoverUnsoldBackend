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
    public async Task<UrlPage<DistributorInformationDto>> GetDistributors([FromQuery] DistributorFilterDto distributorFilterDto)
    {
        var urlPaginationParam = new UrlPaginationParameter(
            distributorFilterDto.PerPage, distributorFilterDto.Page,
            this.GetCleanUrl(), nameof(distributorFilterDto.Page), nameof(distributorFilterDto.PerPage)
        );
        return await _distributorsService.GetDistributors(urlPaginationParam, distributorFilterDto.Name);
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