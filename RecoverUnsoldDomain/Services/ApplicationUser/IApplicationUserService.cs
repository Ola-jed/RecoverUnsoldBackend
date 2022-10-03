using RecoverUnsoldDomain.Dto;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldDomain.Services.ApplicationUser;

public interface IApplicationUserService
{
    Task<User?> FindById(Guid id);
    Task<User?> FindByIdWithFcmTokens(Guid id);
    Task<User?> FindByEmail(string email);
    Task<User?> FindByUsername(string username);
    Task<User?> FindByEmailOrUsername(string emailOrUsername);
    Task<bool> IsEmailVerified(string email);
    Task<bool> EmailExists(string email);
    Task<bool> UsernameExists(string username);
    Task UpdateCustomer(Guid userId, CustomerUpdateDto customerUpdateDto);
    Task UpdateDistributor(Guid userId, DistributorUpdateDto distributorUpdateDto);
    Task UpdatePassword(Guid userId, string password);
    Task Delete(Guid userId);
}