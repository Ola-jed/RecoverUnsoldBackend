using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Reports;

public interface IReportsService
{
    Task ReportDistributor(Guid customerId, Guid distributorToReportId, ReportCreateDto reportCreateDto);
}