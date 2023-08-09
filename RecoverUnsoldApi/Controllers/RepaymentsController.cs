using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldApi.Services.Repayments;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Distributor)]
public class RepaymentsController : ControllerBase
{
    private readonly IRepaymentsService _repaymentsService;

    public RepaymentsController(IRepaymentsService repaymentsService)
    {
        _repaymentsService = repaymentsService;
    }

    [HttpGet]
    public async Task<Page<RepaymentReadDto>> GetRepayments([FromQuery] RepaymentFilterDto repaymentFilterDto)
    {
        var paginationParameter = new PaginationParameter(repaymentFilterDto.PerPage, repaymentFilterDto.Page);
        return await _repaymentsService.GetRepayments(this.GetUserId(), paginationParameter, repaymentFilterDto);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RepaymentReadDto>> GetRepayment(Guid id)
    {
        var repayment = await _repaymentsService.GetForUser(id, this.GetUserId());
        return repayment == null ? NotFound() : repayment;
    }
}