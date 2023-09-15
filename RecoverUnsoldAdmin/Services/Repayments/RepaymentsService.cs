using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Services.Repayments;

public class RepaymentsService : IRepaymentsService
{
    private readonly IDbContextFactory<DataContext> _dbContextFactory;

    public RepaymentsService(IDbContextFactory<DataContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Page<Repayment>> GetRepayments(RepaymentsFilter repaymentsFilter)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        var query = context.Repayments
            .Include(r => r.Order)
            .ThenInclude(o => o!.Offer)
            .ThenInclude(o => o!.Distributor)
            .AsQueryable();

        if (repaymentsFilter.Done != null)
        {
            query = query.Where(r => r.Done == repaymentsFilter.Done);
        }

        if (repaymentsFilter.Search != null)
        {
            query = query.Where(r =>
                EF.Functions.Like(r.Order!.Offer!.Distributor!.Username, $"%{repaymentsFilter.Search}%"));
        }

        var paginationParameter = new PaginationParameter(repaymentsFilter.PerPage, repaymentsFilter.Page);
        return await query.AsSplitQuery()
            .AsyncPaginate(paginationParameter, r => r.CreatedAt);
    }

    public async Task MarkAsDone(Guid id, RepaymentValidationModel repaymentValidationModel)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        await context.Repayments
            .Where(r => r.Id == id)
            .ExecuteUpdateAsync(r =>
                r.SetProperty(x => x.Done, true)
                    .SetProperty(x => x.Note, repaymentValidationModel.Note)
                    .SetProperty(x => x.TransactionId, repaymentValidationModel.TransactionId)
            );
    }
}