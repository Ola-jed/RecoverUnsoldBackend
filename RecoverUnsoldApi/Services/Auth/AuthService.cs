using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecoverUnsoldApi.Data;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BCrypt.Net.BCrypt;

namespace RecoverUnsoldApi.Services.Auth;

public class AuthService : IAuthService
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
        var distributor = new Distributor
        {
            Username = distributorRegisterDto.Username,
            Email = distributorRegisterDto.Email,
            Password = HashPassword(distributorRegisterDto.Password),
            Rccm = distributorRegisterDto.Rccm,
            TaxId = distributorRegisterDto.TaxId,
            Phone = distributorRegisterDto.Phone,
            WebsiteUrl = distributorRegisterDto.WebsiteUrl
        };
        _context.Distributors.Add(distributor);
        await _context.SaveChangesAsync();
    }

    public async Task<JwtSecurityToken?> Login(LoginDto loginDto)
    {
        var user = await ValidateCredentials(loginDto);
        return user == null ? null : GenerateJwt(user);
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

    public async Task<bool> AreCredentialsValid(string email, string password)
    {
        var userPassword = await _context.Users
            .Where(u => u.Email == email)
            .Select(u => u.Password)
            .FirstOrDefaultAsync();
        return Verify(password, userPassword ?? "");
    }

    public JwtSecurityToken GenerateJwt(User user)
    {
        var role = user switch
        {
            Customer    => Roles.Customer,
            Distributor => Roles.Distributor,
            _           => throw new InvalidOperationException()
        };
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(CustomClaims.Id, user.Id.ToString()),
            new(ClaimTypes.Role, role)
        };
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var jwtToken = new JwtSecurityToken(
            expires: DateTime.Now.AddDays(30),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        return jwtToken;
    }
}