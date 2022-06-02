using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Services.ApplicationUser;
using RecoverUnsoldApi.Services.ForgotPassword;
using RecoverUnsoldApi.Services.Mail;

namespace RecoverUnsoldApi.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class ForgotPasswordController: ControllerBase
{
    private readonly IForgotPasswordService _forgotPasswordService;
    private readonly IApplicationUserService _applicationUserService;
    private readonly IMailService _mailService;
    
    public ForgotPasswordController(IForgotPasswordService forgotPasswordService,
        IApplicationUserService applicationUserService, IMailService mailService)
    {
        _forgotPasswordService = forgotPasswordService;
        _applicationUserService = applicationUserService;
        _mailService = mailService;
    }

    [HttpPost("Start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _applicationUserService.FindByEmail(forgotPasswordDto.Email);
        if (user == null)
        {
            return NotFound();
        }

        var token = await _forgotPasswordService.CreateResetPasswordToken(user);
        // TODO : Send mail
        return Ok();
    }

    [HttpPost("Reset")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Reset(PasswordResetDto passwordResetDto)
    {
        var passwordResetSuccessful = await _forgotPasswordService
                .ResetUserPassword(passwordResetDto.Token, passwordResetDto.Password);
        return passwordResetSuccessful ? Ok() : BadRequest();
    }
}