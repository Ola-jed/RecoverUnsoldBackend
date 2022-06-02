using RecoverUnsoldApi.Entities;

namespace RecoverUnsoldApi.Services.UserVerification;

public interface IUserVerificationService
{
    Task<bool> IsEmailConfirmed(string email);
    Task<string> GenerateUserVerificationToken(User user);
    Task<bool> VerifyUser(string token);
}