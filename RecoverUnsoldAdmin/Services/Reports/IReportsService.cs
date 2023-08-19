using FluentPaginator.Lib.Page;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Services.Reports;

public interface IReportsService
{
    Task<Page<Report>> GetReports(ReportsFilter reportsFilter);
    Task MarkAsProcessed(Guid id, bool processed);
}