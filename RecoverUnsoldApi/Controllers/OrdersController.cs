using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.ApplicationUser;
using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldApi.Services.Mail.Mailable;
using RecoverUnsoldApi.Services.Notification.NotificationMessage;
using RecoverUnsoldApi.Services.Offers;
using RecoverUnsoldApi.Services.Orders;
using RecoverUnsoldApi.Services.Queue;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IApplicationUserService _applicationUserService;
    private readonly IOffersService _offersService;
    private readonly IOrdersService _ordersService;
    private readonly IQueueService _queueService;

    public OrdersController(IOrdersService ordersService, IApplicationUserService applicationUserService,
        IQueueService queueService, IOffersService offersService)
    {
        _ordersService = ordersService;
        _applicationUserService = applicationUserService;
        _queueService = queueService;
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
    [HttpGet("Customer")]
    public async Task<Page<OrderReadDto>> GetCustomerOrders([FromQuery] OrderFilterDto orderFilterDto)
    {
        var paginationParam = new PaginationParameter(orderFilterDto.PerPage, orderFilterDto.Page);
        return await _ordersService.GetCustomerOrders(this.GetUserId(), paginationParam, orderFilterDto);
    }

    [Authorize(Roles = Roles.Distributor)]
    [HttpGet("Distributor")]
    public async Task<Page<OrderReadDto>> GetDistributorReceivedOrders([FromQuery] OrderFilterDto orderFilterDto)
    {
        var paginationParam = new PaginationParameter(orderFilterDto.PerPage, orderFilterDto.Page);
        return await _ordersService.GetDistributorOrders(this.GetUserId(), paginationParam, orderFilterDto);
    }

    [Authorize(Roles = Roles.Distributor)]
    [HttpGet("/api/Offers/{id:guid}/Orders")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Page<OrderReadDto>>> GetOfferOffers(Guid id,
        [FromQuery] OrderFilterDto orderFilterDto)
    {
        var distributorId = this.GetUserId();
        var isOfferOwner = await _offersService.IsOwner(distributorId, id);
        if (!isOfferOwner) return Forbid();

        var paginationParam = new PaginationParameter(orderFilterDto.PerPage, orderFilterDto.Page);
        return await _ordersService.GetOfferOrders(this.GetUserId(), paginationParam, orderFilterDto);
    }

    [Authorize(Roles = Roles.Customer)]
    [HttpPost("/api/Offers/{id:guid}/Orders")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderReadDto>> MakeOrder(Guid id, OrderCreateDto orderCreateDto)
    {
        if (!await _offersService.Exists(id)) return NotFound();

        if (!await _ordersService.IsOrderRequestValid(id)) return Forbid();

        if (!await _ordersService.IsOrderRequestInDateInterval(id, orderCreateDto.WithdrawalDate)) return BadRequest();

        var customer = (await _applicationUserService.FindByIdWithFcmTokens(this.GetUserId()))!;
        var orderDto = await _ordersService.CreateOrder(orderCreateDto, customer.Id, id);
        var order = (await _ordersService.GetOrder(orderDto.Id))!;
        var distributor =
            (await _applicationUserService.FindByIdWithFcmTokens(order.Offer?.DistributorId ?? Guid.Empty))!;
        var offerPublishDate = order.Offer!.CreatedAt;
        var offerValidatedMail = new OfferValidatedMail(customer.Username, customer.Email);
        var orderMadeMail = new OrderMadeMail(offerPublishDate, distributor.Username, distributor.Email);
        _queueService.QueueMail(offerValidatedMail.BuildMailMessage());
        _queueService.QueueMail(orderMadeMail.BuildMailMessage());

        var customerTokens = customer.FcmTokens.Select(t => t.Value).ToList();
        _queueService.QueueFirebaseMessage(new OfferValidatedNotificationMessage(customerTokens)
            .BuildFirebaseMessage());
        var distributorTokens = distributor.FcmTokens.Select(t => t.Value).ToList();
        _queueService.QueueFirebaseMessage(new OrderMadeNotificationMessage(offerPublishDate, distributorTokens)
            .BuildFirebaseMessage());
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
        if (order == null) return NotFound();

        var isOwner = await _ordersService.IsRelativeToDistributor(id, this.GetUserId());
        if (!isOwner) return Forbid();

        var relatedOffer = order.Offer!;
        var customer = order.Customer!;
        var customerEntity = (await _applicationUserService.FindByIdWithFcmTokens(customer.Id))!;
        await _ordersService.Accept(id);
        var orderAcceptedMail = new OrderAcceptedMail(customer.Username, order.CreatedAt, relatedOffer.Price,
            relatedOffer.CreatedAt, order.WithdrawalDate, customer.Email);
        _queueService.QueueMail(orderAcceptedMail.BuildMailMessage());
        var customerTokens = customerEntity.FcmTokens.Select(t => t.Value).ToList();
        _queueService.QueueFirebaseMessage(
            new OrderAcceptedNotificationMessage(order.CreatedAt, relatedOffer.Price, relatedOffer.CreatedAt,
                customerTokens).BuildFirebaseMessage()
        );
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
        if (order == null) return NotFound();

        var isOwner = await _ordersService.IsRelativeToDistributor(id, this.GetUserId());
        if (!isOwner) return Forbid();

        var relatedOffer = order.Offer!;
        var customer = order.Customer!;
        var customerEntity = (await _applicationUserService.FindByIdWithFcmTokens(customer.Id))!;
        await _ordersService.Reject(id);
        var orderRejectedMail = new OrderRejectedMail(customer.Username, order.CreatedAt, relatedOffer.Price,
            relatedOffer.CreatedAt, customer.Email);
        _queueService.QueueMail(orderRejectedMail.BuildMailMessage());
        var customerTokens = customerEntity.FcmTokens.Select(t => t.Value).ToList();
        _queueService.QueueFirebaseMessage(
            new OrderRejectedNotificationMessage(order.CreatedAt, relatedOffer.Price, relatedOffer.CreatedAt,
                customerTokens).BuildFirebaseMessage()
        );
        return NoContent();
    }

    [Authorize(Roles = Roles.Distributor)]
    [HttpPost("{id:guid}/Complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CompleteOrder(Guid id)
    {
        var order = await _ordersService.GetOrder(id);
        if (order == null) return NotFound();

        var isOwner = await _ordersService.IsRelativeToDistributor(id, this.GetUserId());
        if (!isOwner) return Forbid();

        var relatedOffer = order.Offer!;
        var customer = order.Customer!;
        var customerEntity = (await _applicationUserService.FindByIdWithFcmTokens(customer.Id))!;

        await _ordersService.Complete(id);
        var orderCompletedMail = new OrderCompletedMail(customer.Username, order.CreatedAt, relatedOffer.Price,
            relatedOffer.CreatedAt, customer.Email);
        _queueService.QueueMail(orderCompletedMail.BuildMailMessage());
        var customerTokens = customerEntity.FcmTokens.Select(t => t.Value).ToList();
        _queueService.QueueFirebaseMessage(
            new OrderCompletedNotificationMessage(order.CreatedAt, relatedOffer.Price, relatedOffer.CreatedAt,
                customerTokens).BuildFirebaseMessage()
        );
        return NoContent();
    }

    [Authorize(Roles = Roles.Customer)]
    [HttpPost("{id:guid}/Invoice")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> SendInvoice(Guid id)
    {
        var invoiceMail = await _ordersService.GetInvoiceMail(id, this.GetUserId());

        if (invoiceMail == null) return NotFound();
        _queueService.QueueMail(invoiceMail.BuildMailMessage(), QueueConstants.PriorityHigh);
        return NoContent();
    }
}