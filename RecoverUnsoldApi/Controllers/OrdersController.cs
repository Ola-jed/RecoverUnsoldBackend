using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.ApplicationUser;
using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldApi.Services.Mail;
using RecoverUnsoldApi.Services.Mail.Mailable;
using RecoverUnsoldApi.Services.Offers;
using RecoverUnsoldApi.Services.Orders;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersService _ordersService;
    private readonly IOffersService _offersService;
    private readonly IApplicationUserService _applicationUserService;
    private readonly IMailService _mailService;

    public OrdersController(IOrdersService ordersService, IApplicationUserService applicationUserService,
        IMailService mailService, IOffersService offersService)
    {
        _ordersService = ordersService;
        _applicationUserService = applicationUserService;
        _mailService = mailService;
        _offersService = offersService;
    }

    [Authorize]
    [HttpGet("{id:guid}", Name = "GetOrder")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OfferReadDto>> GetOrder(Guid id)
    {
        var order = await _ordersService.GetOrder(id);
        return order == null ? NotFound() : Ok(order);
    }

    [Authorize(Roles = Roles.Customer)]
    [HttpPost("/api/Offers/{id:guid}/Orders")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderReadDto>> MakeOrder(Guid id, OrderCreateDto orderCreateDto)
    {
        if (!await _offersService.Exists(id))
        {
            return NotFound();
        }

        if (!await _ordersService.IsOrderRequestValid(id))
        {
            return BadRequest();
        }

        var customer = (await _applicationUserService.FindById(this.GetUserId()))!;
        var orderDto = await _ordersService.CreateOrder(orderCreateDto, customer.Id, id);
        var order = (await _ordersService.GetOrder(orderDto.Id))!;
        var distributor = (await _applicationUserService.FindById(order.Offer?.DistributorId ?? Guid.Empty))!;
        var offerValidatedMail = new OfferValidatedMail(customer.Username, customer.Email);
        var orderMadeMail = new OrderMadeMail(order.Offer!.CreatedAt, distributor.Username, distributor.Email);
        await _mailService.TrySend(offerValidatedMail);
        await _mailService.TrySend(orderMadeMail);
        return CreatedAtRoute(nameof(GetOrder), new { id = order.Id }, order);
    }
}