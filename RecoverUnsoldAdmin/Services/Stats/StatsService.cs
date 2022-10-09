using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities.Enums;

namespace RecoverUnsoldAdmin.Services.Stats;

public class StatsService : IStatsService
{
    private readonly IDbContextFactory<DataContext> _dbContextFactory;

    public StatsService(IDbContextFactory<DataContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Models.Stats> GetStats()
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        return new Models.Stats(
            await context.Customers.CountAsync(),
            await context.Distributors.CountAsync(),
            await context.Offers.CountAsync(),
            await context.Orders.CountAsync(),
            await context.Orders.Include(o => o.Offer)
                .CountAsync(),
            await context.Orders.Include(o => o.Offer)
                .Where(o => o.Status == Status.Completed)
                .SumAsync(o => o.Offer!.Price),
            await context.Offers.GroupBy(log => log.CreatedAt.Date)
                .OrderBy(g => g.Key)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.Date, g => g.Count)
        );
    }
}