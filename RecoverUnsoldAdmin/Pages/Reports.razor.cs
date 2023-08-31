using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldAdmin.Services.AccountSuspensions;
using RecoverUnsoldAdmin.Services.Reports;
using RecoverUnsoldAdmin.Shared;
using RecoverUnsoldAdmin.Utils;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Pages;

public class ReportsBase : ComponentBase
{
    protected bool Loading { get; private set; } = true;
    protected MudTable<Report>? Table { get; set; }
    protected ReportsFilter Filter { get; set; } = new();
    protected Guid SelectedDistributorId { get; set; }

    [Inject]
    protected IStringLocalizer<App> StringLocalizer { get; set; } = default!;

    [Inject]
    protected IReportsService ReportsService { get; set; } = default!;

    [Inject]
    protected IDialogService DialogService { get; set; } = default!;

    [Inject]
    protected ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    protected IAccountSuspensionsService AccountSuspensionsService { get; set; } = default!;

    protected HashSet<Guid> ExpandedRows { get; set; } = new();

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

    protected void Expand(Guid id)
    {
        if (ExpandedRows.Contains(id))
        {
            ExpandedRows.Remove(id);
        }
        else
        {
            ExpandedRows.Add(id);
        }
    }

    protected async Task OpenAccountSuspensionDialog(Guid distributorId)
    {
        SelectedDistributorId = distributorId;
        var parameters = new DialogParameters<AccountSuspensionDialog>
        {
            { x => x.DistributorId, distributorId }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, CloseOnEscapeKey = true };
        var dialog = await DialogService.ShowAsync<AccountSuspensionDialog>(
            StringLocalizer["SuspendDistributor"],
            parameters,
            options
        );
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            Snackbar.Add(StringLocalizer["DistributorAccountSuspendedSuccessfully"], Severity.Success);
        }
    }

    protected async Task RevokeAccountSuspension(Guid suspensionId)
    {
        await AccountSuspensionsService.RevokeSuspension(suspensionId);
        Snackbar.Add(StringLocalizer["AccountSuspensionRevokedSuccessfully"], Severity.Success);
        await Table!.ReloadServerData();
    }
}