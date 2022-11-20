﻿using FluentPaginator.Lib.Page;
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
    Task<Page<OrderReadDto>> GetCustomerOrders(Guid customerId, PaginationParameter paginationParameter, OrderFilterDto orderFilterDto);
    Task<Page<OrderReadDto>> GetOfferOrders(Guid offerId, PaginationParameter paginationParameter, OrderFilterDto orderFilterDto);
    Task<Page<OrderReadDto>> GetDistributorOrders(Guid distributorId, PaginationParameter paginationParameter, OrderFilterDto orderFilterDto);
    Task<OrderReadDto> CreateOrder(OrderCreateDto orderCreateDto, Guid customerId, Guid offerId);
    Task Accept(Guid orderId);
    Task Reject(Guid orderId);
    Task Complete(Guid orderId);
}