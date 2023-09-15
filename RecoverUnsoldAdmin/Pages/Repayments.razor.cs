using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldAdmin.Services.Repayments;
using RecoverUnsoldAdmin.Shared;
using RecoverUnsoldAdmin.Utils;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Pages;

public class RepaymentsBase : ComponentBase
{
    protected bool Loading { get; private set; } = true;
    protected MudTable<Repayment>? Table { get; set; }
    protected RepaymentsFilter Filter { get; set; } = new();
    protected Guid SelectedRepaymentId { get; set; }

    [Inject]
    protected IStringLocalizer<App> StringLocalizer { get; set; } = default!;

    [Inject]
    protected IRepaymentsService RepaymentsService { get; set; } = default!;

    [Inject]
    protected IDialogService DialogService { get; set; } = default!;

    [Inject]
    protected ISnackbar Snackbar { get; set; } = default!;

    protected async Task OnSearch()
    {
        await Table!.ReloadServerData();
    }

    protected async Task ResetFilters()
    {
        Filter = new RepaymentsFilter();
        await Table!.ReloadServerData();
    }

    protected async Task ToggleDone(string value)
    {
        Filter.Done = value switch
        {
            DoneLabels.Done => true,
            DoneLabels.Pending => false,
            _ => null
        };

        await Table!.ReloadServerData();
    }

    protected async Task<TableData<Repayment>> ServerData(TableState arg)
    {
        Filter.Page = arg.Page + 1;
        Filter.PerPage = arg.PageSize;
        var reportsPage = await RepaymentsService.GetRepayments(Filter);
        Loading = false;
        return new TableData<Repayment> { Items = reportsPage.Items, TotalItems = reportsPage.Total };
    }

    public async Task MarkAsDone(Guid id)
    {
        SelectedRepaymentId = id;
        var parameters = new DialogParameters<RepaymentDialog>
        {
            { x => x.RepaymentId, id }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, CloseOnEscapeKey = true };
        var dialog = await DialogService.ShowAsync<RepaymentDialog>(
            StringLocalizer["ProcessRepayment"],
            parameters,
            options
        );
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            Snackbar.Add(StringLocalizer["DistributorAccountSuspendedSuccessfully"], Severity.Success);
        }
    }
}