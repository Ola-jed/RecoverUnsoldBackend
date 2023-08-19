using Microsoft.EntityFrameworkCore;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Services.AccountSuspensions;

public class AccountSuspensionsService : IAccountSuspensionsService
{
    private readonly IDbContextFactory<DataContext> _dbContextFactory;

    public AccountSuspensionsService(IDbContextFactory<DataContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task SuspendAccount(Guid distributorId, AccountSuspensionModel accountSuspensionModel)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        context.AccountSuspensions.Add(new AccountSuspension
        {
            Reason = accountSuspensionModel.Reason,
            Date = accountSuspensionModel.Date,
            EndDate = accountSuspensionModel.EndDate,
            Active = true,
            DistributorId = distributorId
        });

        await context.SaveChangesAsync();
    }

    public async Task RevokeSuspension(Guid suspensionId)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        await context.AccountSuspensions
            .Where(a => a.Id == suspensionId)
            .ExecuteUpdateAsync(accountSuspension => accountSuspension.SetProperty(x => x.Active, false));
    }
}