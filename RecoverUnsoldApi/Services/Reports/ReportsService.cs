using RecoverUnsoldApi.Dto;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldApi.Services.Reports;

public class ReportsService : IReportsService
{
    private readonly DataContext _context;

    public ReportsService(DataContext dataContext)
    {
        _context = dataContext;
    }

    public async Task ReportDistributor(Guid customerId, Guid distributorToReportId, ReportCreateDto reportCreateDto)
    {
        _context.Reports.Add(new Report
        {
            CustomerId = customerId,
            ReportedDistributorId = distributorToReportId,
            Reason = reportCreateDto.Reason,
            Description = reportCreateDto.Description
        });

        await _context.SaveChangesAsync();
    }
}