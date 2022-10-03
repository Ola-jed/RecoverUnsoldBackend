using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldDomain.Services.ForgotPassword;

public class ForgotPasswordService : IForgotPasswordService
{
    private const int TokenLifetimeInMinutes = 10;
    private readonly DataContext _context;

    public ForgotPasswordService(DataContext context)
    {
        _context = context;
    }

    public async Task<string> CreateResetPasswordToken(User user)
    {
        string token;
        do
        {
            token = RandomNumberGenerator.GetInt32(10000, 100000).ToString();
        } while (_context.PasswordResets.Any(p => p.Token == token));

        _context.PasswordResets.Add(new PasswordReset
        {
            UserId = user.Id,
            Token = token
        });

        await _context.SaveChangesAsync();
        return token;
    }

    public async Task<bool> ResetUserPassword(string token, string newPassword)
    {
        var passwordReset = await _context.PasswordResets
            .FirstOrDefaultAsync(p => p.Token == token);
        if (passwordReset == null)
        {
            return false;
        }

        if (passwordReset.CreatedAt.AddMinutes(TokenLifetimeInMinutes) < DateTime.Now)
        {
            _context.PasswordResets.Remove(passwordReset);
            await _context.SaveChangesAsync();
            return false;
        }

        var user = await _context.Users.FindAsync(passwordReset.UserId);
        user!.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        _context.Users.Update(user);
        _context.PasswordResets.Remove(passwordReset);
        await _context.SaveChangesAsync();
        return true;
    }
}