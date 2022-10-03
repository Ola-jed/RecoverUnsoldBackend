using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldDomain.Services.UserVerification;

public class DummyUserVerificationService : IUserVerificationService
{
    private readonly DataContext _context;

    public DummyUserVerificationService(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> IsEmailConfirmed(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email && x.EmailVerifiedAt != null);
    }

    public async Task<string> GenerateUserVerificationToken(User user)
    {
        const string token = "123456";
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

        var user = await _context.Users.FindAsync(emailVerification.UserId);
        user!.EmailVerifiedAt = DateTime.Now;
        _context.Users.Update(user);
        _context.EmailVerifications.Remove(emailVerification);
        await _context.SaveChangesAsync();
        return true;
    }
}