using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldDomain.Dto;
using RecoverUnsoldDomain.Entities.Enums;
using RecoverUnsoldDomain.Extensions;
using RecoverUnsoldDomain.Services.Alerts;
using RecoverUnsoldDomain.Services.Auth;

namespace RecoverUnsoldApi.Controllers;

[ApiController]
[Authorize(Roles = Roles.Customer)]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly IAlertsService _alertsService;

    public AlertsController(IAlertsService alertsService)
    {
        _alertsService = alertsService;
    }

    [HttpGet]
    public async Task<IEnumerable<AlertReadDto>> GetAlerts()
    {
        return await _alertsService.GetAlerts(this.GetUserId());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateAlert(AlertCreateDto alertCreateDto)
    {
        if ((alertCreateDto.AlertType == AlertType.AnyOfferPublished && alertCreateDto.DistributorId != null) ||
            (alertCreateDto.AlertType == AlertType.DistributorOfferPublished && alertCreateDto.DistributorId == null))
        {
            return BadRequest();
        }

        var customerId = this.GetUserId();
        if (alertCreateDto.DistributorId == null)
        {
            await _alertsService.CreateAlertForAllOffers(customerId);
        }
        else
        {
            await _alertsService.CreateForDistributorOffers(customerId, (Guid)alertCreateDto.DistributorId);
        }

        return NoContent();
    }

    [HttpDelete("{alertId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAlert(Guid alertId)
    {
        var customerId = this.GetUserId();
        var isOwner = await _alertsService.IsOwnedByUser(alertId, customerId);
        if (!isOwner)
        {
            return NotFound();
        }

        await _alertsService.DeleteAlert(alertId);
        return NoContent();
    }
}