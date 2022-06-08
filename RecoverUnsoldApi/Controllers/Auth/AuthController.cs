using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Services.ApplicationUser;
using RecoverUnsoldApi.Services.Auth;

namespace RecoverUnsoldApi.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IApplicationUserService _applicationUserService;

    public AuthController(IAuthService authService, IApplicationUserService applicationUserService)
    {
        _authService = authService;
        _applicationUserService = applicationUserService;
    }

    [HttpPost("Register/Customer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RegisterCustomer(CustomerRegisterDto customerRegisterDto)
    {
        await _authService.RegisterCustomer(customerRegisterDto);
        return Ok();
    }

    [HttpPost("Register/Distributor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RegisterDistributor(DistributorRegisterDto distributorRegisterDto)
    {
        await _authService.RegisterDistributor(distributorRegisterDto);
        return Ok();
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<TokenDto>> Login(LoginDto loginDto)
    {
        var token = await _authService.Login(loginDto);
        if (token == null)
        {
            return Unauthorized();
        }

        var isVerified = await _applicationUserService.IsEmailVerified(loginDto.Email);
        if (!isVerified)
        {
            return Forbid();
        }

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        var role = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        return new TokenDto(tokenString, role, token.ValidTo);
    }
}