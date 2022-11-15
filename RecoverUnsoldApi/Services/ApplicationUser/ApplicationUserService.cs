using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldDomain.Entities;

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

    public async Task<User?> FindByIdWithFcmTokens(Guid id)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.FcmTokens)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> FindByEmail(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> FindByUsername(string username)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task<User?> FindByEmailOrUsername(string emailOrUsername)
    {
        return await FindByEmail(emailOrUsername) ?? await FindByUsername(emailOrUsername);
    }

    public async Task<bool> IsEmailVerified(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(x => x.Email == email && x.EmailVerifiedAt != null);
    }

    public async Task<bool> EmailExists(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(x => x.Email == email);
    }

    public async Task<bool> UsernameExists(string username)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(x => x.Username == username);
    }

    public async Task UpdateCustomer(Guid userId, CustomerUpdateDto customerUpdateDto)
    {
        await _context.Customers
            .Where(c => c.Id == userId)
            .ExecuteUpdateAsync(
                customer => customer.SetProperty(x => x.Username, customerUpdateDto.Username)
                    .SetProperty(x => x.FirstName, customerUpdateDto.FirstName)
                    .SetProperty(x => x.LastName, customerUpdateDto.LastName)
            );
    }

    public async Task UpdateDistributor(Guid userId, DistributorUpdateDto distributorUpdateDto)
    {
        await _context.Distributors
            .Where(c => c.Id == userId)
            .ExecuteUpdateAsync(
                distributor => distributor.SetProperty(x => x.Username, distributorUpdateDto.Username)
                    .SetProperty(x => x.Phone, distributorUpdateDto.Phone)
                    .SetProperty(x => x.TaxId, distributorUpdateDto.TaxId)
                    .SetProperty(x => x.Rccm, distributorUpdateDto.Rccm)
                    .SetProperty(x => x.WebsiteUrl, distributorUpdateDto.WebsiteUrl)
            );
    }

    public async Task UpdatePassword(Guid userId, string password)
    {
        var passwordHashed = BCrypt.Net.BCrypt.HashPassword(password);
        await _context.Users
            .Where(c => c.Id == userId)
            .ExecuteUpdateAsync(user => user.SetProperty(x => x.Password, passwordHashed));
    }

    public async Task Delete(Guid userId)
    {
        await _context.Users
            .Where(x => x.Id == userId)
            .ExecuteDeleteAsync();
    }
}