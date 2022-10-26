using System.Security.Claims;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldDomain.Data;

namespace RecoverUnsoldAdmin.Services;

public class AppAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IDbContextFactory<DataContext> _dbContextFactory;
    private readonly ISessionStorageService _sessionStorageService;
    private const string EmailKey = "1";
    private const string UsernameKey = "2";
    private const string AuthenticationType = "Auth";

    public AppAuthenticationStateProvider(IDbContextFactory<DataContext> dbContextFactory,
        ISessionStorageService sessionStorageService)
    {
        _dbContextFactory = dbContextFactory;
        _sessionStorageService = sessionStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var email = await _sessionStorageService.GetItemAsync<string>(EmailKey);
        var username = await _sessionStorageService.GetItemAsync<string>(UsernameKey);
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
        await _sessionStorageService.SetItemAsync(EmailKey, authenticatedAdmin.Email);
        await _sessionStorageService.SetItemAsync(UsernameKey, authenticatedAdmin.Username);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        return true;
    }

    public async Task Logout()
    {
        await _sessionStorageService.ClearAsync();
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