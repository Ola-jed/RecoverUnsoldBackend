using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Services.Auth;

namespace RecoverUnsoldApi.Extensions;

public static class ControllerBaseExtensions
{
    public static Guid GetUserId<T>(this T controller) where T : ControllerBase
    {
        return Guid.Parse(controller.User.FindFirst(CustomClaims.Id)?.Value!);
    }
}