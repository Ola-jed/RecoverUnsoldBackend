using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using RecoverUnsoldApi.Services.Auth;

namespace RecoverUnsoldApi.Extensions;

public static class ControllerBaseExtensions
{
    public static string GetCleanUrl<T>(this T controller) where T : ControllerBase
    {
        return controller.Request.GetDisplayUrl().Split('?')[0];
    }

    public static Guid GetUserId<T>(this T controller) where T: ControllerBase
    {
        return Guid.Parse(controller.User.FindFirst(CustomClaims.Id)?.Value!);
    }
}