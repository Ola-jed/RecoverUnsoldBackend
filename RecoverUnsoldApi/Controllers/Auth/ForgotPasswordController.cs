using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Services.ApplicationUser;
using RecoverUnsoldApi.Services.ForgotPassword;
using RecoverUnsoldApi.Services.Mail.Mailable;
using RecoverUnsoldApi.Services.Queue;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class ForgotPasswordController : ControllerBase
{
    private readonly IApplicationUserService _applicationUserService;
    private readonly IForgotPasswordService _forgotPasswordService;
    private readonly IQueueService _queueService;

    public ForgotPasswordController(IForgotPasswordService forgotPasswordService,
        IApplicationUserService applicationUserService, IQueueService queueService)
    {
        _forgotPasswordService = forgotPasswordService;
        _applicationUserService = applicationUserService;
        _queueService = queueService;
    }

    [HttpPost("Start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _applicationUserService.FindByEmail(forgotPasswordDto.Email);
        if (user == null) return NotFound();

        var token = await _forgotPasswordService.CreateResetPasswordToken(user);
        var forgotPasswordMail = new ForgotPasswordMail(user.Username, token, user.Email);
        _queueService.QueueMail(forgotPasswordMail.BuildMailMessage(), QueueConstants.PriorityHigh);
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