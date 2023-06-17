using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldApi.Tests.Helpers;

namespace RecoverUnsoldApi.Tests.Services;

public class AuthServiceTest
{
    [Fact]
    public async Task TestUserRegistration()
    {
        var authService = new AuthService(DbContextHelper.GetDbContext(), new ConfigurationHelper().GetConfiguration());
    }
}