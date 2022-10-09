using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldDomain.Services.FcmTokens;

public class FcmTokensService: IFcmTokensService
{
    private readonly DataContext _context;

    public FcmTokensService(DataContext context)
    {
        _context = context;
    }
    
    public async Task Create(Guid userId, string value)
    {
        var sameTokenExists = await _context.FcmTokens.AnyAsync(x => x.Value == value);
        if (sameTokenExists)
        {
            return;
        }
        
        _context.FcmTokens.Add(new FcmToken
        {
            Value = value,
            UserId = userId
        });
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAllForUser(Guid userId)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM \"FcmTokens\" WHERE \"UserId\" =  {userId}");
    }

    public async Task Remove(string value)
    {
        var token = await _context.FcmTokens.FirstOrDefaultAsync(t => t.Value == value);
        if(token == null) return;
        _context.FcmTokens.Remove(token);
        await _context.SaveChangesAsync();
    }
}