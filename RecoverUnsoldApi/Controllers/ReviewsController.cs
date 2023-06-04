using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.ApplicationUser;
using RecoverUnsoldApi.Services.Mail.Mailable;
using RecoverUnsoldApi.Services.Queue;
using RecoverUnsoldApi.Services.Reviews;
using RecoverUnsoldDomain.Config;
using RecoverUnsoldDomain.Queue;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReviewsController : ControllerBase
{
    private readonly AppOwner _appOwner;
    private readonly IQueueService _queueService;
    private readonly IReviewsService _reviewsService;
    private readonly IApplicationUserService _userService;

    public ReviewsController(IOptions<AppOwner> options, IReviewsService reviewsService, IQueueService queueService,
        IApplicationUserService userService)
    {
        _appOwner = options.Value;
        _reviewsService = reviewsService;
        _queueService = queueService;
        _userService = userService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Publish(ReviewCreateDto reviewCreateDto)
    {
        var userId = this.GetUserId();
        var review = await _reviewsService.Publish(userId, reviewCreateDto.Comment);
        var authenticatedUser = (await _userService.FindById(userId))!;
        var reviewPublishedMail = new ReviewPublishedMail(_appOwner, authenticatedUser.Username,
            authenticatedUser.Email, review.Comment);
        _queueService.QueueMail(reviewPublishedMail.BuildMailMessage(), QueueConstants.PriorityLow);
        return NoContent();
    }
}