using RecoverUnsoldApi.Entities;

namespace RecoverUnsoldApi.Services.ApplicationUser;

public interface IApplicationUserService
{
    Task<User?> FindById(Guid id);
    Task<User?> FindByEmail(string email);
    Task<User?> FindByUsername(string username);
    Task<User?> FindByEmailOrUsername(string emailOrUsername);
    Task<bool> IsEmailVerified(string email);
    Task<bool> EmailExists(string email);
    Task<bool> UsernameExists(string username);
}