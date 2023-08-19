using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldAdmin.Services.AccountSuspensions;
using RecoverUnsoldAdmin.Services.Reports;
using RecoverUnsoldAdmin.Utils;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Pages;

public class ReportsBase : ComponentBase
{
    protected bool Loading { get; private set; } = true;
    protected MudTable<Report>? Table { get; set; }
    protected ReportsFilter Filter { get; set; } = new();

    [Inject]
    protected IStringLocalizer<App> StringLocalizer { get; set; } = default!;

    [Inject]
    protected IReportsService ReportsService { get; set; } = default!;

    [Inject]
    protected IAccountSuspensionsService AccountSuspensionsService { get; set; } = default!;

    [Inject]
    protected ISnackbar Snackbar { get; set; } = default!;

    protected async Task OnSearch()
    {
        await Table!.ReloadServerData();
    }

    protected async Task ResetFilters()
    {
        Filter = new ReportsFilter();
        await Table!.ReloadServerData();
    }

    protected async Task<TableData<Report>> ServerData(TableState arg)
    {
        Filter.Page = arg.Page + 1;
        Filter.PerPage = arg.PageSize;
        var reportsPage = await ReportsService.GetReports(Filter);
        Loading = false;
        return new TableData<Report> { Items = reportsPage.Items, TotalItems = reportsPage.Total };
    }

    protected async Task ToggleProcessed(string value)
    {
        Filter.Processed = value switch
        {
            ProcessedLabels.Processed => true,
            ProcessedLabels.NonProcessed => false,
            _ => null
        };

        await Table!.ReloadServerData();
    }

    protected async Task MarkAsProcessed(Guid reportId, bool processed)
    {
        await ReportsService.MarkAsProcessed(reportId, processed);
        Snackbar.Add(StringLocalizer["ReportProcessedSuccessfully"], Severity.Success);
        await Table!.ReloadServerData();
    }
}