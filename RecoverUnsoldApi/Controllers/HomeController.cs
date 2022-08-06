using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldApi.Services.Home;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController: ControllerBase
{
    private readonly IHomeService _homeService;

    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    [HttpGet]
    public async Task<CustomerHomeDto> GetCustomerHome()
    {
        return await _homeService.GetCustomerHomeInformation();
    }
    
    [Authorize(Roles = Roles.Distributor)]
    [HttpGet("Distributors")]
    public async Task<DistributorHomeDto> GetDistributorHome([FromQuery] PeriodDto period)
    {
        return await _homeService.GetDistributorHomeInformation(this.GetUserId(), period.PeriodStart, period.PeriodEnd);
    }
}