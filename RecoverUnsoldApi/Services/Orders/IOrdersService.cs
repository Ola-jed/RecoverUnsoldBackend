using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Orders;

public interface IOrdersService
{
    Task<bool> IsOrderRequestValid(Guid offerId);
    Task<bool> IsOrderRequestInDateInterval(Guid offerId, DateTime dateTime);
    Task<OrderReadDto?> GetOrder(Guid id);
    Task<UrlPage<OrderReadDto>> GetCustomerOrders(Guid customerId, UrlPaginationParameter urlPaginationParameter);
    Task<UrlPage<OrderReadDto>> GetOfferOrders(Guid offerId, UrlPaginationParameter urlPaginationParameter);
    Task<UrlPage<OrderReadDto>> GetDistributorOrders(Guid distributorId, UrlPaginationParameter urlPaginationParameter);
    Task<OrderReadDto> CreateOrder(OrderCreateDto orderCreateDto,Guid customerId, Guid offerId);
}