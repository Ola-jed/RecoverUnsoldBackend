using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;

namespace RecoverUnsoldApi.Services.AccountSuspensions;

public class AccountSuspensionsService : IAccountSuspensionsService
{
    private readonly DataContext _context;

    public AccountSuspensionsService(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> IsUserAccountCurrentlySuspended(Guid id)
    {
        return await _context.AccountSuspensions
            .Where(a => a.DistributorId == id && a.Active && (a.EndDate == null || a.EndDate > DateTime.Now))
            .AnyAsync();
    }
}