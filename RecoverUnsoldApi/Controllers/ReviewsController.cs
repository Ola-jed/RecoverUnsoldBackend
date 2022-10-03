using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RecoverUnsoldDomain.Config;
using RecoverUnsoldDomain.Dto;
using RecoverUnsoldDomain.Extensions;
using RecoverUnsoldDomain.Services.ApplicationUser;
using RecoverUnsoldDomain.Services.Mail;
using RecoverUnsoldDomain.Services.Mail.Mailable;
using RecoverUnsoldDomain.Services.Reviews;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReviewsController : ControllerBase
{
    private readonly AppOwner _appOwner;
    private readonly IReviewsService _reviewsService;
    private readonly IMailService _mailService;
    private readonly IApplicationUserService _userService;

    public ReviewsController(IOptions<AppOwner> options, IReviewsService reviewsService, IMailService mailService,
        IApplicationUserService userService)
    {
        _appOwner = options.Value;
        _reviewsService = reviewsService;
        _mailService = mailService;
        _userService = userService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Publish(ReviewCreateDto reviewCreateDto)
    {
        var userId = this.GetUserId();
        var review = await _reviewsService.Publish(userId, reviewCreateDto.Comment);
        var authenticatedUser = (await _userService.FindById(userId))!;
        await _mailService.TrySend(new ReviewPublishedMail(_appOwner, authenticatedUser.Username,
            authenticatedUser.Email, review.Comment));
        return NoContent();
    }
}