using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Services.ApplicationUser;
using RecoverUnsoldApi.Services.Mail.Mailable;
using RecoverUnsoldApi.Services.Queue;
using RecoverUnsoldApi.Services.UserVerification;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class UserVerificationController : ControllerBase
{
    private readonly IApplicationUserService _applicationUserService;
    private readonly IQueueService _queueService;
    private readonly IUserVerificationService _userVerificationService;

    public UserVerificationController(IUserVerificationService userVerificationService,
        IApplicationUserService applicationUserService, IQueueService queueService)
    {
        _userVerificationService = userVerificationService;
        _applicationUserService = applicationUserService;
        _queueService = queueService;
    }

    [HttpPost("Start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> StartUserVerificationProcess(UserVerificationStartDto userVerificationStartDto)
    {
        var user = await _applicationUserService.FindByEmail(userVerificationStartDto.Email);
        if (user == null) return NotFound();

        var isAlreadyVerified = await _userVerificationService.IsEmailConfirmed(userVerificationStartDto.Email);
        if (isAlreadyVerified) return BadRequest();

        var token = await _userVerificationService.GenerateUserVerificationToken(user);
        var userVerificationMail = new UserVerificationMail(user.Username, token, user.Email);
        _queueService.QueueMail(userVerificationMail.BuildMailMessage(), QueueConstants.PriorityHigh);
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