using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldApi.Services.ForgotPassword;

public class DummyForgotPasswordService : IForgotPasswordService
{
    private readonly DataContext _context;

    public DummyForgotPasswordService(DataContext context)
    {
        _context = context;
    }

    public async Task<string> CreateResetPasswordToken(User user)
    {
        const string token = "123456";
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

        var user = await _context.Users.FindAsync(passwordReset.UserId);
        user!.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        _context.Users.Update(user);
        _context.PasswordResets.Remove(passwordReset);
        await _context.SaveChangesAsync();
        return true;
    }
}