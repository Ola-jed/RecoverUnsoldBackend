using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldApi.Services.FcmTokens;

public class FcmTokensService : IFcmTokensService
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
        await _context.FcmTokens
            .Where(t => t.UserId == userId)
            .ExecuteDeleteAsync();
    }

    public async Task Remove(string value)
    {
        await _context.FcmTokens
            .Where(t => t.Value == value)
            .ExecuteDeleteAsync();
    }
}