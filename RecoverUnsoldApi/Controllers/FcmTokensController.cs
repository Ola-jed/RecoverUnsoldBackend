using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.FcmTokens;

namespace RecoverUnsoldApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FcmTokensController : ControllerBase
{
    private readonly IFcmTokensService _fcmTokensService;

    public FcmTokensController(IFcmTokensService fcmTokensService)
    {
        _fcmTokensService = fcmTokensService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Create(FcmTokenCreateDto fcmTokenCreateDto)
    {
        await _fcmTokensService.Create(this.GetUserId(), fcmTokenCreateDto.Value);
        return NoContent();
    }

    [HttpDelete("{value}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(string value)
    {
        await _fcmTokensService.Remove(value);
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAll()
    {
        await _fcmTokensService.RemoveAllForUser(this.GetUserId());
        return NoContent();
    }
}