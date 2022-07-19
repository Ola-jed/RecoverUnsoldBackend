using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Data;
using RecoverUnsoldApi.Dto;
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

    public async Task<User?> FindByIdWithFcmTokens(Guid id)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.FcmTokens)
            .FirstOrDefaultAsync(x => x.Id == id);
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

    public async Task UpdateCustomer(Guid userId, CustomerUpdateDto customerUpdateDto)
    {
        var customer = await _context.Customers.FindAsync(userId);
        if (customer == null)
        {
            return;
        }

        customer.Username = customerUpdateDto.Username;
        customer.FirstName = customerUpdateDto.FirstName;
        customer.LastName = customerUpdateDto.LastName;
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDistributor(Guid userId, DistributorUpdateDto distributorUpdateDto)
    {
        var distributor = await _context.Distributors.FindAsync(userId);
        if (distributor == null)
        {
            return;
        }

        distributor.Username = distributorUpdateDto.Username;
        distributor.Phone = distributorUpdateDto.Phone;
        distributor.TaxId = distributorUpdateDto.TaxId;
        distributor.Rccm = distributorUpdateDto.Rccm;
        distributor.WebsiteUrl = distributorUpdateDto.WebsiteUrl;
        _context.Distributors.Update(distributor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePassword(Guid userId, string password)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return;
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(password);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}