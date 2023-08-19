using RecoverUnsoldAdmin.Models;

namespace RecoverUnsoldAdmin.Services.AccountSuspensions;

public interface IAccountSuspensionsService
{
    Task SuspendAccount(Guid distributorId, AccountSuspensionModel accountSuspensionModel);
    Task RevokeSuspension(Guid suspensionId);
}