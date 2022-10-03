using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldDomain.Dto;
using RecoverUnsoldDomain.Services.ApplicationUser;
using RecoverUnsoldDomain.Services.Mail;
using RecoverUnsoldDomain.Services.Mail.Mailable;
using RecoverUnsoldDomain.Services.UserVerification;

namespace RecoverUnsoldApi.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class UserVerificationController : ControllerBase
{
    private readonly IUserVerificationService _userVerificationService;
    private readonly IApplicationUserService _applicationUserService;
    private readonly IMailService _mailService;

    public UserVerificationController(IUserVerificationService userVerificationService,
        IApplicationUserService applicationUserService, IMailService mailService)
    {
        _userVerificationService = userVerificationService;
        _applicationUserService = applicationUserService;
        _mailService = mailService;
    }

    [HttpPost("Start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> StartUserVerificationProcess(UserVerificationStartDto userVerificationStartDto)
    {
        var user = await _applicationUserService.FindByEmail(userVerificationStartDto.Email);
        if (user == null)
        {
            return NotFound();
        }

        var isAlreadyVerified = await _userVerificationService.IsEmailConfirmed(userVerificationStartDto.Email);
        if (isAlreadyVerified)
        {
            return BadRequest();
        }

        var token = await _userVerificationService.GenerateUserVerificationToken(user);
        var userVerificationMail = new UserVerificationMail(user.Username, token, user.Email);
        await _mailService.SendEmailAsync(userVerificationMail);
        return Ok();
    }

    [HttpPost("Verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> VerifyUser(UserVerificationDto userVerificationDto)
    {
        var userVerified = await _userVerificationService.VerifyUser(userVerificationDto.Token);
        return userVerified ? Ok() : BadRequest();
    }
}