using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecoverUnsoldApi.Data;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Entities;
using static BCrypt.Net.BCrypt;

namespace RecoverUnsoldApi.Services.Auth;

public class AuthService: IAuthService
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;
    
    public AuthService(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task RegisterCustomer(CustomerRegisterDto customerRegisterDto)
    {
        var password = HashPassword(customerRegisterDto.Password);
        var customer = new Customer
        {
            Username = customerRegisterDto.Username,
            Email = customerRegisterDto.Email,
            Password = password
        };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task RegisterDistributor(DistributorRegisterDto distributorRegisterDto)
    {
        var password = HashPassword(distributorRegisterDto.Password);
        var distributor = new Distributor
        {
            Username = distributorRegisterDto.UserName,
            Email = distributorRegisterDto.Email,
            Password = password,
            Rccm = distributorRegisterDto.Rccm,
            Ifu = distributorRegisterDto.Ifu,
            Phone = distributorRegisterDto.Phone,
            WebsiteUrl = distributorRegisterDto.WebsiteUrl
        };
        _context.Distributors.Add(distributor);
        await _context.SaveChangesAsync();
    }

    public async Task<JwtSecurityToken?> Login(LoginDto loginDto)
    {
        var user = await ValidateCredentials(loginDto);
        if (user == null)
        {
            return null;
        }

        return await GenerateJwt(user);
    }

    public async Task<User?> ValidateCredentials(LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null)
        {
            return null;
        }

        return Verify(loginDto.Password, user.Password) ? user : null;
    }

    public async Task<JwtSecurityToken> GenerateJwt(User user)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(CustomClaims.Id, user.Id.ToString())
        };
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var jwtToken = new JwtSecurityToken(
            expires: DateTime.Now.AddDays(30),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        return await Task.Run(() => jwtToken);
    }
}