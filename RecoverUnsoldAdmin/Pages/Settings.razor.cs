using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using RecoverUnsoldAdmin.Extensions;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldAdmin.Services;

namespace RecoverUnsoldAdmin.Pages;

public class SettingsBase : ComponentBase
{
    [Inject]
    protected IStringLocalizer<App> StringLocalizer { get; set; } = default!;

    [Inject]
    protected AppAuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    [Inject]
    private NavigationManager NavManager { get; set; } = default!;

    protected ClaimsPrincipal? User { get; set; }
    protected bool EditingUsername { get; set; }
    protected bool EditingEmail { get; set; }
    protected bool IsValid { get; set; } = true;
    protected bool Loading { get; set; }
    protected string[] Errors { get; set; } = { };

    protected bool IsDirty => (EditingEmail || EditingUsername) &&
                              (User?.Email() != AccountUpdateModel.Email ||
                               User?.Name() != AccountUpdateModel.Username);

    protected AccountUpdateModel AccountUpdateModel { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await InitUser();
    }
    
    private async Task InitUser()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        User = authState.User;
        AccountUpdateModel.Username = authState.User.Name();
        AccountUpdateModel.Email = authState.User.Email();
    }

    protected void ToggleEditingUsername()
    {
        EditingUsername = !EditingUsername;
        if (!EditingUsername)
        {
            AccountUpdateModel.Username = User!.Name();
        }
    }

    protected void ToggleEditingEmail()
    {
        EditingEmail = !EditingEmail;
        if (!EditingEmail)
        {
            AccountUpdateModel.Email = User!.Email();
        }
    }

    protected async Task Submit()
    {
        Loading = true;
        if (!EditingEmail)
        {
            AccountUpdateModel.Email = User!.Email();
        }

        if (!EditingUsername)
        {
            AccountUpdateModel.Username = User!.Name();
        }

        await AuthenticationStateProvider.UpdateAccount(AccountUpdateModel);
        NavManager.NavigateTo(NavManager.Uri, true);
        Loading = false;
    }
}