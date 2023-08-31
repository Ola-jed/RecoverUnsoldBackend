using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Services.Reports;

public class ReportsService : IReportsService
{
    private readonly IDbContextFactory<DataContext> _dbContextFactory;

    public ReportsService(IDbContextFactory<DataContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Page<Report>> GetReports(ReportsFilter reportsFilter)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        var query = context.Reports
            .Include(r => r.Customer)
            .Include(r => r.ReportedDistributor)
            .ThenInclude(d => d!.Reports)
            .Include(r => r.ReportedDistributor)
            .ThenInclude(d => d!.AccountSuspensions)
            .AsSplitQuery();

        if (reportsFilter.Processed != null)
        {
            query = query.Where(r => r.Processed == reportsFilter.Processed);
        }

        if (reportsFilter.Search != null)
        {
            query = query.Where(r =>
                EF.Functions.Like(r.Reason, $"%{reportsFilter.Search}%") ||
                EF.Functions.Like(r.Customer!.Username, $"%{reportsFilter.Search}%") ||
                EF.Functions.Like(r.ReportedDistributor!.Username, $"%{reportsFilter.Search}%"));
        }

        var paginationParameter = new PaginationParameter(reportsFilter.PerPage, reportsFilter.Page);
        return await query
            .AsyncPaginate(paginationParameter, r => r.CreatedAt);
    }

    public async Task MarkAsProcessed(Guid id, bool processed)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        await context.Reports
            .Where(r => r.Id == id)
            .ExecuteUpdateAsync(r => r.SetProperty(x => x.Processed, processed));
    }
}