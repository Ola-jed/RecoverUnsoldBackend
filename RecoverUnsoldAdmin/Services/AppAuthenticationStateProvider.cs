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
        var id = await _localStorageService.GetItemAsync<string>(StorageItemKeys.IdKey);
        if (email == null || username == null || id == null)
        {
            return new AuthenticationState(new ClaimsPrincipal());
        }

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, username),
            new Claim(CustomClaims.Id, id)
        }, AuthenticationType);
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
        await _localStorageService.SetItemAsync(StorageItemKeys.IdKey, authenticatedAdmin.Id);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        return true;
    }

    public async Task Logout()
    {
        await _localStorageService.RemoveItemsAsync(new[]
        {
            StorageItemKeys.EmailKey,
            StorageItemKeys.UsernameKey
        });
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task UpdateAccount(AccountUpdateModel accountUpdateModel)
    {
        var id = Guid.Parse(await _localStorageService.GetItemAsync<string>(StorageItemKeys.IdKey));
        var context = await _dbContextFactory.CreateDbContextAsync();
        await context.Administrators
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(admin => admin.SetProperty(x => x.Email, accountUpdateModel.Username)
                .SetProperty(x => x.Username, accountUpdateModel.Username)
            );

        await _localStorageService.SetItemAsync(StorageItemKeys.EmailKey, accountUpdateModel.Email);
        await _localStorageService.SetItemAsync(StorageItemKeys.UsernameKey, accountUpdateModel.Username);
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, accountUpdateModel.Email),
            new Claim(ClaimTypes.Name, accountUpdateModel.Username),
            new Claim(CustomClaims.Id, id.ToString())
        }, AuthenticationType);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
    }

    private async Task<AuthDetails?> ValidateCredentialsAndGetUser(AuthenticationModel authenticationModel)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        var admin = await context
            .Administrators
            .AsNoTracking()
            .Where(x => x.Email == authenticationModel.Email)
            .Select(x => new AuthDetails(x.Id, x.Email, x.Username, x.Password))
            .FirstOrDefaultAsync();

        if (admin == null)
        {
            return null;
        }

        return BCrypt.Net.BCrypt.Verify(authenticationModel.Password, admin.Password) ? admin : null;
    }

    private static ClaimsPrincipal BuildIdentity(AuthDetails administrator)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, administrator.Email),
            new Claim(ClaimTypes.Name, administrator.Username),
            new Claim(CustomClaims.Id, administrator.Id.ToString())
        }, AuthenticationType);
        return new ClaimsPrincipal(identity);
    }
}