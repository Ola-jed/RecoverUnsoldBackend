using System.Data;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Data;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using Serilog;

namespace RecoverUnsoldApi.Services.Home;

public class HomeService : IHomeService
{
    private readonly DataContext _context;

    public HomeService(DataContext context)
    {
        _context = context;
    }

    public async Task<CustomerHomeDto> GetCustomerHomeInformation(Guid? customerId)
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

        return customerId == null
            ? new CustomerHomeDto(offers, distributors, null)
            : new CustomerHomeDto(offers, distributors, await GetCustomerOrderStats(customerId.Value));
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

    private async Task<CustomerOrderStatsDto?> GetCustomerOrderStats(Guid customerId)
    {
        var databaseConnection = _context.Database.GetDbConnection();
        try
        {
            await databaseConnection.OpenAsync();
            await using var command = databaseConnection.CreateCommand();
            command.CommandText = "select count(\"Orders\".\"Id\"),sum(O.\"Price\") from \"Orders\" inner join \"Offers\" O on O.\"Id\" = \"Orders\".\"OfferId\" where \"Orders\".\"CustomerId\" = @CustomerId";
            command.CommandType = CommandType.Text;
            
            var customerParameter = command.CreateParameter();
            customerParameter.ParameterName = "@CustomerId";
            customerParameter.Value = customerId;
            customerParameter.DbType = DbType.Guid;
            command.Parameters.Add(customerParameter);
            
            await using var dataReader = await command.ExecuteReaderAsync();
            return dataReader.HasRows
                ? new CustomerOrderStatsDto(dataReader.GetInt32(0), dataReader.GetDecimal(1))
                : new CustomerOrderStatsDto(0, 0);
        }
        catch (Exception exception)
        {
            Log.Logger.Fatal(exception,"Error when doing operation");
            return null;
        }
        finally
        {
            await databaseConnection.CloseAsync();
        }
    }
}