using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Localization.CookieRequestCultureProvider;

namespace RecoverUnsoldAdmin.Controllers;

[Route("[controller]")]
public class CultureController : ControllerBase
{
    [HttpGet]
    public IActionResult SetCulture([FromQuery] string culture)
    {
        HttpContext.Response.Cookies.Append(
            DefaultCookieName,
            MakeCookieValue(new Microsoft.AspNetCore.Localization.RequestCulture(culture))
        );
        return LocalRedirect("/");
    }
}