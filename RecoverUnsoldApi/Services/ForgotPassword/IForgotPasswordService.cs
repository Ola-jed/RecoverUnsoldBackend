using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldApi.Services.ForgotPassword;

public interface IForgotPasswordService
{
    Task<string> CreateResetPasswordToken(User user);
    Task<bool> ResetUserPassword(string token, string newPassword);
}