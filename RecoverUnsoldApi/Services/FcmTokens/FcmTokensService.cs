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
        var id = Guid.NewGuid();
        var now = DateTime.Now;

        await _context.Database.ExecuteSqlInterpolatedAsync(
            $@"INSERT INTO ""FcmTokens"" (""Id"", ""Value"", ""UserId"", ""CreatedAt"") VALUES ({id}, {value}, {userId}, {now}) ON CONFLICT(""Value"") DO NOTHING");
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