using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldAdmin.Utils;
using RecoverUnsoldDomain.Data;

namespace RecoverUnsoldAdmin.Services;

public class AppAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IDbContextFactory<DataContext> _dbContextFactory;
    private readonly ILocalStorageService _localStorageService;
    private const string AuthenticationType = "Auth";

    public AppAuthenticationStateProvider(IDbContextFactory<DataContext> dbContextFactory,
        ILocalStorageService localStorageService)
    {
        _dbContextFactory = dbContextFactory;
        _localStorageService = localStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var email = await _localStorageService.GetItemAsync<string>(StorageItemKeys.EmailKey);
        var username = await _localStorageService.GetItemAsync<string>(StorageItemKeys.UsernameKey);
        if (email == null || username == null)
        {
            return new AuthenticationState(new ClaimsPrincipal());
        }

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, username)
        }, authenticationType: AuthenticationType);
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task<bool> Authenticate(AuthenticationModel authenticationModel)
    {
        var authenticatedAdmin = await ValidateCredentialsAndGetUser(authenticationModel);
        if (authenticatedAdmin == null)
        {
            return false;
        }

        var user = BuildIdentity(authenticatedAdmin);
        await _localStorageService.SetItemAsync(StorageItemKeys.EmailKey, authenticatedAdmin.Email);
        await _localStorageService.SetItemAsync(StorageItemKeys.UsernameKey, authenticatedAdmin.Username);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        return true;
    }

    public async Task Logout()
    {
        await _localStorageService.RemoveItemsAsync(new []
        {
            StorageItemKeys.EmailKey,
            StorageItemKeys.UsernameKey
        });
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    private async Task<AdministratorAuthDetails?> ValidateCredentialsAndGetUser(AuthenticationModel authenticationModel)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        var admin = await context
            .Administrators
            .AsNoTracking()
            .Where(x => x.Email == authenticationModel.Email)
            .Select(x => new AdministratorAuthDetails(x.Email, x.Username, x.Password))
            .FirstOrDefaultAsync();
        
        if (admin == null)
        {
            return null;
        }

        return BCrypt.Net.BCrypt.Verify(authenticationModel.Password, admin.Password) ? admin : null;
    }

    private static ClaimsPrincipal BuildIdentity(AdministratorAuthDetails administrator)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, administrator.Email),
            new Claim(ClaimTypes.Name, administrator.Username)
        }, authenticationType: AuthenticationType);
        return new ClaimsPrincipal(identity);
    }
}