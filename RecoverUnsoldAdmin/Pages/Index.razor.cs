using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using RecoverUnsoldAdmin.Models;
using RecoverUnsoldAdmin.Services.Stats;

namespace RecoverUnsoldAdmin.Pages;

public class IndexBase : ComponentBase
{
    [Inject]
    private IStatsService StatsService { get; set; } = default!;

    [Inject]
    protected IStringLocalizer<App> StringLocalizer { get; set; } = default!;

    protected bool Loading { get; private set; } = true;
    protected Stats? Stats { get; set; }

    protected ChartOptions Options { get; } = new()
    {
        YAxisTicks = 1,
        InterpolationOption = InterpolationOption.Straight
    };

    protected List<ChartSeries> Series { get; set; } = default!;
    protected string[] Labels { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Stats = await StatsService.GetStats();
        Labels = Stats.OffersPublishedPerDay
            .Keys
            .OrderBy(x => x.Date)
            .Select(x => x.Date.ToShortDateString())
            .ToArray();
        Series = new List<ChartSeries>(2)
        {
            new()
            {
                Name = StringLocalizer["OfferPublishedPerDay"],
                Data = Stats.OffersPublishedPerDay
                    .OrderBy(x => x.Key.Date)
                    .Select(x => (double)x.Value)
                    .ToArray()
            },
            new()
            {
                Name = StringLocalizer["OrdersPerDay"],
                Data = Stats.OrdersPerDay
                    .OrderBy(x => x.Key.Date)
                    .Select(x => (double)x.Value)
                    .ToArray()
            }
        };
        Loading = false;
    }
}