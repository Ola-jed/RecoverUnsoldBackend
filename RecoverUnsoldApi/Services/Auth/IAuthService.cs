using System.IdentityModel.Tokens.Jwt;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldApi.Services.Auth;

public interface IAuthService
{
    Task RegisterCustomer(CustomerRegisterDto customerRegisterDto);
    Task RegisterDistributor(DistributorRegisterDto distributorRegisterDto);
    Task<(JwtSecurityToken, UserDataDto)?> Login(LoginDto loginDto);
    Task<User?> ValidateCredentials(LoginDto loginDto);
    Task<bool> AreCredentialsValid(string email, string password);
    JwtSecurityToken GenerateJwt(User user);
}