using RecoverUnsoldApi.Data;

namespace RecoverUnsoldApi.Services.Orders;

public class OrdersService : IOrdersService
{
    private readonly DataContext _context;
    
    public OrdersService(DataContext context)
    {
        _context = context;
    }
}