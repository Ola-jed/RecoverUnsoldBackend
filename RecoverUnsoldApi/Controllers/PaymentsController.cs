using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldApi.Services.Orders;
using RecoverUnsoldApi.Services.Payments;
using RecoverUnsoldDomain.Entities.Enums;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Authorize(Roles = Roles.Customer)]
public class PaymentsController : ControllerBase
{
    private readonly IOrdersService _ordersService;
    private readonly IPaymentsService _paymentsService;

    public PaymentsController(IOrdersService ordersService, IPaymentsService paymentsService)
    {
        _ordersService = ordersService;
        _paymentsService = paymentsService;
    }

    [HttpPost("api/Orders/{id:guid}/Pay")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> MakePayment(Guid id, TransactionDto transactionDto)
    {
        if (!await _ordersService.IsRelativeToCustomer(id, this.GetUserId())) return Forbid();

        if (!await _ordersService.MatchStatuses(id, new[] { Status.Approved, Status.Completed })) return BadRequest();

        var transactionId = transactionDto.TransactionId;

        // TODO : fix the issue with Kkiapay always returning TRANSACTION_NOT_FOUND
        // if (!await _paymentsService.CheckPaymentSuccess(transactionId))
        // {
        //     return BadRequest();
        // }
        await _paymentsService.CreatePayment(id, transactionId);
        return Ok();
    }
}