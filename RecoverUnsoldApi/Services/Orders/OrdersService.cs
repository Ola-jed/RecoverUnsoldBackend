using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Data;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Entities;
using RecoverUnsoldApi.Entities.Enums;
using RecoverUnsoldApi.Extensions;

namespace RecoverUnsoldApi.Services.Orders;

public class OrdersService : IOrdersService
{
    private readonly DataContext _context;

    public OrdersService(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> IsOrderRequestValid(Guid offerId)
    {
        var beneficiaries = await _context.Offers
            .Where(o => o.Id == offerId)
            .Select(o => o.Beneficiaries)
            .FirstOrDefaultAsync();
        var ordersValidated = await _context.Orders
            .CountAsync(o => o.OfferId == offerId && (o.Status == Status.Approved || o.Status == Status.Pending));
        return beneficiaries == null || ordersValidated < beneficiaries;
    }

    public async Task<bool> IsOrderRequestInDateInterval(Guid offerId, DateTime dateTime)
    {
        return await _context.Offers
            .AnyAsync(o => o.Id == offerId && dateTime > o.StartDate && o.StartDate.AddSeconds(o.Duration) >= dateTime);
    }

    public async Task<OrderReadDto?> GetOrder(Guid id)
    {
        var order = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Customer)
            .Include(o => o.Opinions)
            .Include(o => o.Offer)
            .ThenInclude(o => o!.Location)
            .FirstOrDefaultAsync(o => o.Id == id);
        return order?.ToOrderReadDto();
    }

    public async Task<UrlPage<OrderReadDto>> GetCustomerOrders(Guid customerId,
        UrlPaginationParameter urlPaginationParameter, OrderFilterDto orderFilterDto)
    {
        return await Task.Run(() => _context.Orders
            .AsNoTracking()
            .Include(o => o.Offer)
            .Include(o => o.Customer)
            .Include(o => o.Opinions)
            .Where(o => o.CustomerId == customerId)
            .ApplyFilters(orderFilterDto)
            .ToOrderReadDto()
            .UrlPaginate(urlPaginationParameter, o => o, PaginationOrder.Descending));
    }

    public async Task<UrlPage<OrderReadDto>> GetOfferOrders(Guid offerId, UrlPaginationParameter urlPaginationParameter,
        OrderFilterDto orderFilterDto)
    {
        return await Task.Run(() => _context.Orders
            .AsNoTracking()
            .Include(o => o.Offer)
            .Include(o => o.Customer)
            .Include(o => o.Opinions)
            .Where(o => o.OfferId == offerId)
            .ApplyFilters(orderFilterDto)
            .ToOrderReadDto()
            .UrlPaginate(urlPaginationParameter, o => o, PaginationOrder.Descending));
    }

    public async Task<UrlPage<OrderReadDto>> GetDistributorOrders(Guid distributorId,
        UrlPaginationParameter urlPaginationParameter, OrderFilterDto orderFilterDto)
    {
        return await Task.Run(() => _context.Orders
            .AsNoTracking()
            .Include(o => o.Customer)
            .Include(o => o.Opinions)
            .Include(o => o.Offer)
            .Where(o => o.Offer != null && o.Offer.DistributorId == distributorId)
            .ApplyFilters(orderFilterDto)
            .ToOrderReadDto()
            .UrlPaginate(urlPaginationParameter, o => o, PaginationOrder.Descending));
    }

    public async Task<OrderReadDto> CreateOrder(OrderCreateDto orderCreateDto, Guid customerId, Guid offerId)
    {
        var order = new Order
        {
            WithdrawalDate = orderCreateDto.WithdrawalDate,
            CustomerId = customerId,
            OfferId = offerId
        };
        var entityEntry = _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return entityEntry.Entity.ToOrderReadDto();
    }
}