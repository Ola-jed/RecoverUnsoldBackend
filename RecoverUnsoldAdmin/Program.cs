using Blazored.LocalStorage;
using Microsoft.AspNetCore.Localization;
using MudBlazor.Services;
using RecoverUnsoldAdmin.Extensions;
using RecoverUnsoldAdmin.Services;
using RecoverUnsoldAdmin.Services.Customers;
using RecoverUnsoldAdmin.Services.Distributors;
using RecoverUnsoldAdmin.Services.Offers;
using RecoverUnsoldAdmin.Services.Stats;
using RecoverUnsoldAdmin.Utils;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.ConfigureDbContext(configuration);
builder.Services.ConfigureGoogleMaps(configuration);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<IDistributorsService, DistributorsService>();
builder.Services.AddScoped<ICustomersService, CustomersService>();
builder.Services.AddScoped<IOffersService, OffersService>();
builder.Services.AddScoped<IStatsService, StatsService>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = AppCulture.SupportedCultures,
    SupportedUICultures = AppCulture.SupportedCultures
});
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapControllers();
app.MapFallbackToPage("/_Host");
app.Run();