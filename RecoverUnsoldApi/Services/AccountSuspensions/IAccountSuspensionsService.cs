namespace RecoverUnsoldApi.Services.AccountSuspensions;

public interface IAccountSuspensionsService
{
    Task<bool> IsUserAccountCurrentlySuspended(Guid id);
}