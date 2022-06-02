using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Data;
using RecoverUnsoldApi.Entities;

namespace RecoverUnsoldApi.Services.ApplicationUser;

public class ApplicationUserService : IApplicationUserService
{
    private readonly DataContext _context;

    public ApplicationUserService(DataContext context)
    {
        _context = context;
    }

    public async Task<User?> FindById(Guid id)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> FindByEmail(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> FindByUsername(string username)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task<User?> FindByEmailOrUsername(string emailOrUsername)
    {
        return await FindByEmail(emailOrUsername) ?? await FindByUsername(emailOrUsername);
    }

    public async Task<bool> IsEmailVerified(string email)
    {
        return await _context.Users.AsNoTracking().AnyAsync(x => x.Email == email && x.EmailVerifiedAt != null);
    }

    public async Task<bool> EmailExists(string email)
    {
        return await _context.Users.AsNoTracking().AnyAsync(x => x.Email == email);
    }

    public async Task<bool> UsernameExists(string username)
    {
        return await _context.Users.AsNoTracking().AnyAsync(x => x.Username == username);
    }
}