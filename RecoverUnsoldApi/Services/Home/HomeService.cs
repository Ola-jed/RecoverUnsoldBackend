using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Data;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;

namespace RecoverUnsoldApi.Services.Home;

public class HomeService: IHomeService
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
    
    
}