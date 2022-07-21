using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.ApplicationUser;
using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldApi.Services.Mail;
using RecoverUnsoldApi.Services.Mail.Mailable;
using RecoverUnsoldApi.Services.Notification;
using RecoverUnsoldApi.Services.Notification.NotificationMessage;
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
    private readonly INotificationService _notificationService;

    public OrdersController(IOrdersService ordersService, IApplicationUserService applicationUserService,
        IMailService mailService, IOffersService offersService, INotificationService notificationService)
    {
        _ordersService = ordersService;
        _applicationUserService = applicationUserService;
        _mailService = mailService;
        _offersService = offersService;
        _notificationService = notificationService;
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
    [HttpGet("Customer")]
    public async Task<UrlPage<OrderReadDto>> GetCustomerOrders([FromQuery] OrderFilterDto orderFilterDto)
    {
        var urlPaginationParam = new UrlPaginationParameter(
            orderFilterDto.PerPage, orderFilterDto.Page, this.GetCleanUrl(),
            nameof(orderFilterDto.Page), nameof(orderFilterDto.PerPage)
        );
        return await _ordersService.GetCustomerOrders(this.GetUserId(), urlPaginationParam, orderFilterDto);
    }

    [Authorize(Roles = Roles.Distributor)]
    [HttpGet("Distributor")]
    public async Task<UrlPage<OrderReadDto>> GetDistributorReceivedOrders([FromQuery] OrderFilterDto orderFilterDto)
    {
        var urlPaginationParam = new UrlPaginationParameter(
            orderFilterDto.PerPage, orderFilterDto.Page, this.GetCleanUrl(),
            nameof(orderFilterDto.Page), nameof(orderFilterDto.PerPage)
        );
        return await _ordersService.GetDistributorOrders(this.GetUserId(), urlPaginationParam, orderFilterDto);
    }
    
    [Authorize(Roles = Roles.Distributor)]
    [HttpGet("/api/Offers/{id:guid}/Orders")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<UrlPage<OrderReadDto>>> GetOfferOffers(Guid id,[FromQuery] OrderFilterDto orderFilterDto)
    {
        var urlPaginationParam = new UrlPaginationParameter(
            orderFilterDto.PerPage, orderFilterDto.Page, this.GetCleanUrl(),
            nameof(orderFilterDto.Page), nameof(orderFilterDto.PerPage)
        );
        var distributorId = this.GetUserId();
        var isOfferOwner = await _offersService.IsOwner(distributorId, id);
        if (!isOfferOwner)
        {
            return Forbid();
        }
        
        return await _ordersService.GetOfferOrders(this.GetUserId(), urlPaginationParam, orderFilterDto);
    }
    
    [Authorize(Roles = Roles.Customer)]
    [HttpPost("/api/Offers/{id:guid}/Orders")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderReadDto>> MakeOrder(Guid id, OrderCreateDto orderCreateDto)
    {
        if (!await _offersService.Exists(id))
        {
            return NotFound();
        }

        if (!await _ordersService.IsOrderRequestValid(id))
        {
            return Forbid();
        }

        if (!await _ordersService.IsOrderRequestInDateInterval(id, orderCreateDto.WithdrawalDate))
        {
            return BadRequest();
        }

        var customer = (await _applicationUserService.FindByIdWithFcmTokens(this.GetUserId()))!;
        var orderDto = await _ordersService.CreateOrder(orderCreateDto, customer.Id, id);
        var order = (await _ordersService.GetOrder(orderDto.Id))!;
        var distributor = (await _applicationUserService.FindByIdWithFcmTokens(order.Offer?.DistributorId ?? Guid.Empty))!;
        var offerPublishDate = order.Offer!.CreatedAt;
        var offerValidatedMail = new OfferValidatedMail(customer.Username, customer.Email);
        var orderMadeMail = new OrderMadeMail(offerPublishDate, distributor.Username, distributor.Email);
        await _mailService.TrySend(offerValidatedMail);
        await _mailService.TrySend(orderMadeMail);
        await _notificationService.Send(new OfferValidatedNotificationMessage(), customer);
        await _notificationService.Send(new OrderMadeNotificationMessage(offerPublishDate), distributor);
        return CreatedAtRoute(nameof(GetOrder), new { id = order.Id }, order);
    }
    
    [Authorize(Roles = Roles.Distributor)]
    [HttpPost("{id:guid}/Accept")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AcceptOrder(Guid id)
    {
        var order = await _ordersService.GetOrder(id);
        if (order == null)
        {
            return NotFound();
        }

        var isOwner = await _ordersService.IsRelativeToDistributor(id, this.GetUserId());
        if (!isOwner)
        {
            return Forbid();
        }

        var relatedOffer = order.Offer!;
        var customer = order.Customer!;
        await _ordersService.Accept(id);
        var orderAcceptedMail = new OrderAcceptedMail(customer.Username, order.CreatedAt, relatedOffer.Price,
            relatedOffer.CreatedAt, order.WithdrawalDate, customer.Email);
        await _mailService.SendEmailAsync(orderAcceptedMail);
        return NoContent();
    }
    
    [Authorize(Roles = Roles.Distributor)]
    [HttpPost("{id:guid}/Reject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RejectOrder(Guid id)
    {
        var order = await _ordersService.GetOrder(id);
        if (order == null)
        {
            return NotFound();
        }

        var isOwner = await _ordersService.IsRelativeToDistributor(id, this.GetUserId());
        if (!isOwner)
        {
            return Forbid();
        }

        var relatedOffer = order.Offer!;
        var customer = order.Customer!;
        await _ordersService.Reject(id);
        var orderRejectedMail = new OrderRejectedMail(customer.Username, order.CreatedAt, relatedOffer.Price,
            relatedOffer.CreatedAt, customer.Email);
        await _mailService.SendEmailAsync(orderRejectedMail);
        return NoContent();
    }
}