using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldDomain.Dto;
using RecoverUnsoldDomain.Entities;
using RecoverUnsoldDomain.Extensions;
using RecoverUnsoldDomain.Services.ApplicationUser;
using RecoverUnsoldDomain.Services.Auth;

namespace RecoverUnsoldApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IApplicationUserService _applicationUserService;
    private readonly IAuthService _authService;

    public AccountController(IApplicationUserService applicationUserService, IAuthService authService)
    {
        _applicationUserService = applicationUserService;
        _authService = authService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserReadDto>> GetAccount()
    {
        var user = await _applicationUserService.FindById(this.GetUserId());
        if (user == null)
        {
            return Unauthorized();
        }

        return user switch
        {
            Customer customer       => Ok(customer.ToCustomerReadDto()),
            Distributor distributor => Ok(distributor.ToDistributorReadDto()),
            _                       => Unauthorized()
        };
    }

    [Authorize(Roles = Roles.Customer)]
    [HttpPut("Customer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateCustomer(CustomerUpdateDto customerUpdateDto)
    {
        var userId = this.GetUserId();
        await _applicationUserService.UpdateCustomer(userId, customerUpdateDto);
        return NoContent();
    }

    [Authorize(Roles = Roles.Distributor)]
    [HttpPut("Distributor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateDistributor(DistributorUpdateDto distributorUpdateDto)
    {
        var userId = this.GetUserId();
        await _applicationUserService.UpdateDistributor(userId, distributorUpdateDto);
        return NoContent();
    }

    [HttpPut("Password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> UpdatePassword(PasswordUpdateDto passwordUpdateDto)
    {
        var userId = this.GetUserId();
        var user = (await _applicationUserService.FindById(userId))!;
        var hasValidCredentials = await _authService.AreCredentialsValid(user.Email, passwordUpdateDto.OldPassword);
        if (!hasValidCredentials)
        {
            return Unauthorized();
        }

        await _applicationUserService.UpdatePassword(userId, passwordUpdateDto.NewPassword);
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAccount()
    {
        var userId = this.GetUserId();
        await _applicationUserService.Delete(userId);
        return NoContent();
    }
}