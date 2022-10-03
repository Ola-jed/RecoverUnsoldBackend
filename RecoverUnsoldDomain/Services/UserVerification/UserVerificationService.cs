using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldDomain.Services.UserVerification;

public class UserVerificationService : IUserVerificationService
{
    private const int TokenLifetimeInMinutes = 10;
    private readonly DataContext _context;

    public UserVerificationService(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> IsEmailConfirmed(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email && x.EmailVerifiedAt != null);
    }

    public async Task<string> GenerateUserVerificationToken(User user)
    {
        string token;
        do
        {
            token = RandomNumberGenerator.GetInt32(10000, 100000).ToString();
        } while (_context.PasswordResets.Any(p => p.Token == token));

        _context.EmailVerifications.Add(new EmailVerification
        {
            UserId = user.Id,
            Token = token
        });
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task<bool> VerifyUser(string token)
    {
        var emailVerification = await _context.EmailVerifications
            .FirstOrDefaultAsync(x => x.Token == token);
        if (emailVerification == null)
        {
            return false;
        }

        if (emailVerification.CreatedAt.AddMinutes(TokenLifetimeInMinutes) < DateTime.Now)
        {
            _context.EmailVerifications.Remove(emailVerification);
            await _context.SaveChangesAsync();
            return false;
        }

        var user = await _context.Users.FindAsync(emailVerification.UserId);
        user!.EmailVerifiedAt = DateTime.Now;
        _context.Users.Update(user);
        _context.EmailVerifications.Remove(emailVerification);
        await _context.SaveChangesAsync();
        return true;
    }
}