using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Services.Distributors;

namespace RecoverUnsoldApi.Controllers;

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
    public async Task<Page<DistributorInformationDto>> GetDistributors([FromQuery] DistributorFilterDto distributorFilterDto)
    {
        var paginationParam = new PaginationParameter(distributorFilterDto.PerPage, distributorFilterDto.Page);
        return await _distributorsService.GetDistributors(paginationParam, distributorFilterDto.Name);
    }

    [HttpGet("Labels")]
    public async Task<IEnumerable<DistributorLabelReadDto>> GetDistributorsLabels()
    {
        return await _distributorsService.GetDistributorsLabels();
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