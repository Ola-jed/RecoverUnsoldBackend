using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace RecoverUnsoldAdmin.Pages;

public class LoginBase: ComponentBase
{
    [Inject]
    protected IStringLocalizer<App> StringLocalizer { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;
    
    protected MudForm? Form { get; set; }
    protected MudTextField<string>? EmailField { get; set; }
    protected MudTextField<string>? PasswordField { get; set; }
    protected bool Success { get; set; }
    protected bool Loading { get; set; }
    protected bool LoginFailed { get; set; }
    protected bool PasswordVisible { get; set; }

    protected async Task Submit()
    {
        Loading = true;
        await Task.Delay(3000);
        Loading = false;
    }
}