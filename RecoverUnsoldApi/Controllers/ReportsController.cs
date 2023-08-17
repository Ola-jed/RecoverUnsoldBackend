using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldApi.Services.Distributors;
using RecoverUnsoldApi.Services.Reports;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Authorize(Roles = Roles.Customer)]
public class ReportsController : ControllerBase
{
    private readonly IDistributorsService _distributorsService;
    private readonly IReportsService _reportsService;

    public ReportsController(IDistributorsService distributorsService, IReportsService reportsService)
    {
        _distributorsService = distributorsService;
        _reportsService = reportsService;
    }

    [HttpPost("api/Distributors/{id:guid}/Reports")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ReportDistributor(Guid id, ReportCreateDto reportCreateDto)
    {
        var currentUserId = this.GetUserId();

        if (!await _distributorsService.DistributorExists(id))
        {
            return NotFound();
        }

        await _reportsService.ReportDistributor(currentUserId, id, reportCreateDto);
        return Ok();
    }
}