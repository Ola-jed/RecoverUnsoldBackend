using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
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

    public async Task<Page<Repayment>> ListRepayments(PaginationParameter paginationParameter, bool? done = null)
    {
        throw new NotImplementedException();
    }
}