using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Services.AccountSuspensions;
using RecoverUnsoldApi.Services.ApplicationUser;
using RecoverUnsoldApi.Services.Auth;

namespace RecoverUnsoldApi.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAccountSuspensionsService _accountSuspensionsService;
    private readonly IApplicationUserService _applicationUserService;
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService, IApplicationUserService applicationUserService,
        IAccountSuspensionsService accountSuspensionsService)
    {
        _authService = authService;
        _applicationUserService = applicationUserService;
        _accountSuspensionsService = accountSuspensionsService;
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
    [ProducesResponseType(StatusCodes.Status423Locked)]
    public async Task<ActionResult<AuthenticationResultDto>> Login(LoginDto loginDto)
    {
        var authData = await _authService.Login(loginDto);
        if (authData == null) return Unauthorized();

        var isVerified = await _applicationUserService.IsEmailVerified(loginDto.Email);
        if (!isVerified) return Forbid();
        var jwt = authData.Value.Item1;
        var userData = authData.Value.Item2;

        if (await _accountSuspensionsService.IsUserAccountCurrentlySuspended(userData.Id))
            return StatusCode(StatusCodes.Status423Locked);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);
        return new AuthenticationResultDto(tokenString, userData, jwt.ValidTo);
    }
}