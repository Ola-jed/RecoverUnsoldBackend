using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Data;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;

namespace RecoverUnsoldApi.Services.Home;

public class HomeService : IHomeService
{
    private readonly DataContext _context;

    public HomeService(DataContext context)
    {
        _context = context;
    }

    public async Task<CustomerHomeDto> GetCustomerHomeInformation()
    {
        var now = DateTime.Now;
        var offers = await _context
            .Offers
            .AsNoTracking()
            .Include(o => o.Location)
            .Include(o => o.Products)
            .Where(o => o.StartDate.AddSeconds(o.Duration) > now)
            .OrderByDescending(o => o.CreatedAt)
            .Take(5)
            .ToOfferReadDto()
            .ToListAsync();

        var distributors = await _context
            .Distributors
            .AsNoTracking()
            .OrderBy(_ => EF.Functions.Random())
            .Take(5)
            .ToDistributorInformationReadDto()
            .ToListAsync();

        return new CustomerHomeDto(offers, distributors);
    }

    public async Task<DistributorHomeDto> GetDistributorHomeInformation(Guid distributorId,
        DateTime periodStart, DateTime periodEnd)
    {
        var orders = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Customer)
            .Include(o => o.Opinions)
            .Include(o => o.Offer)
            .Where(o => o.Offer != null && o.Offer.DistributorId == distributorId)
            .OrderByDescending(o => o.CreatedAt)
            .Take(5)
            .ToOrderReadDto()
            .ToListAsync();

        var periodStartDate = periodStart.Date;
        var periodEndDate = periodEnd.Date;
        var ordersPerDay = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Offer)
            .Where(o => o.Offer != null && o.Offer.DistributorId == distributorId)
            .Where(o => o.CreatedAt.Date >= periodStartDate && o.CreatedAt.Date <= periodEndDate)
            .GroupBy(o => o.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToDictionaryAsync(g => g.Date, g => g.Count);

        foreach (var dateTime in Enumerable.Range(0, 1 + periodEnd.Subtract(periodStart).Days)
                     .Select(offset => periodStart.AddDays(offset).Date))
        {
            if (!ordersPerDay.Keys.Contains(dateTime))
            {
                ordersPerDay.Add(dateTime, 0);
            }
        }

        return new DistributorHomeDto(ordersPerDay, orders);
    }
}