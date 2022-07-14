using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Services.Orders;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController: ControllerBase
{
    private readonly IOrdersService _ordersService;
    
    public OrdersController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }
    
    
}