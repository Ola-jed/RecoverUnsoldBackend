using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Orders;

public interface IOrdersService
{
    Task<bool> IsOrderRequestValid(Guid offerId);
    Task<bool> IsRelativeToDistributor(Guid orderId, Guid distributorId);
    Task<bool> IsRelativeToCustomer(Guid orderId, Guid customerId);
    Task<bool> IsOrderRequestInDateInterval(Guid offerId, DateTime dateTime);
    Task<OrderReadDto?> GetOrder(Guid id);

    Task<UrlPage<OrderReadDto>> GetCustomerOrders(Guid customerId, UrlPaginationParameter urlPaginationParameter,
        OrderFilterDto orderFilterDto);

    Task<UrlPage<OrderReadDto>> GetOfferOrders(Guid offerId, UrlPaginationParameter urlPaginationParameter,
        OrderFilterDto orderFilterDto);

    Task<UrlPage<OrderReadDto>> GetDistributorOrders(Guid distributorId, UrlPaginationParameter urlPaginationParameter,
        OrderFilterDto orderFilterDto);

    Task<OrderReadDto> CreateOrder(OrderCreateDto orderCreateDto, Guid customerId, Guid offerId);
    Task Accept(Guid orderId);
    Task Reject(Guid orderId);
    Task Complete(Guid orderId);
}