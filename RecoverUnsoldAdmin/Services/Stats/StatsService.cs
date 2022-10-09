using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities.Enums;

namespace RecoverUnsoldAdmin.Services.Stats;

public class StatsService : IStatsService
{
    private readonly IDbContextFactory<DataContext> _dbContextFactory;

    private const string OffersStatQuery =
        "SELECT date_trunc('day', o.\"CreatedAt\") AS \"Date\", COUNT(*)::INT AS \"Count\" FROM \"Offers\" AS o GROUP BY \"Date\" ORDER BY \"Date\";";

    private const string OrdersStatQuery =
        "SELECT date_trunc('day', o.\"CreatedAt\") AS \"Date\", COUNT(*)::INT AS \"Count\" FROM \"Orders\" AS o GROUP BY \"Date\" ORDER BY \"Date\";";

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
            await GetStatsPerDay(context, StatsType.OffersPerDay),
            await GetStatsPerDay(context, StatsType.OrdersPerDay)
        );
    }

    private static async Task<Dictionary<DateTime, int>> GetStatsPerDay(DbContext context, StatsType statsType)
    {
        var returnData = new Dictionary<DateTime, int>();
        var connection = context.Database.GetDbConnection();
        try
        {
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = statsType == StatsType.OffersPerDay ? OffersStatQuery : OrdersStatQuery;
            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    returnData.Add(reader.GetDateTime(0), reader.GetInt32(1));
                }
            }

            await reader.DisposeAsync();
        }
        finally
        {
            await connection.CloseAsync();
        }

        return returnData;
    }

    private enum StatsType
    {
        OffersPerDay,
        OrdersPerDay
    }
}