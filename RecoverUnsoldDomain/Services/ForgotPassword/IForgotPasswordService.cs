using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldDomain.Services.ForgotPassword;

public interface IForgotPasswordService
{
    Task<string> CreateResetPasswordToken(User user);
    Task<bool> ResetUserPassword(string token, string newPassword);
}