using System.IdentityModel.Tokens.Jwt;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Entities;

namespace RecoverUnsoldApi.Services.Auth;

public interface IAuthService
{
    public Task RegisterCustomer(CustomerRegisterDto customerRegisterDto);
    public Task RegisterDistributor(DistributorRegisterDto distributorRegisterDto);
    public Task<JwtSecurityToken?> Login(LoginDto loginDto);
    public Task<User?> ValidateCredentials(LoginDto loginDto);
    public Task<bool> AreCredentialsValid(string email, string password);
    public Task<JwtSecurityToken> GenerateJwt(User user);
}