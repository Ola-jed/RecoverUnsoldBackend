using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldAdmin.Services;

namespace RecoverUnsoldAdmin.Pages;

public class LoginBase: ComponentBase
{
    [Inject]
    protected IStringLocalizer<App> StringLocalizer { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    protected AppAuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    protected MudForm? Form { get; set; }
    protected AuthenticationModel AuthenticationModel { get; set; } = new();
    protected bool Success { get; set; }
    protected bool Loading { get; set; }
    protected bool LoginFailed { get; set; }
    protected bool PasswordVisible { get; set; }

    protected async Task Submit()
    {
        Loading = true;
        var loginResult = await AuthenticationStateProvider.Authenticate(AuthenticationModel);
        if (loginResult)
        {
            NavigationManager.NavigateTo("/");
            return;
        }
        LoginFailed = true;
        Loading = false;
    }
}