namespace RecoverUnsoldAdmin.Models;

public record Stats(int CustomersCount, int DistributorsCount, int OffersCount, int OrdersCount,
    int FinalizedOrdersCount, decimal TotalSales, Dictionary<DateTime, int> OffersPublishedPerDay);