using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using RecoverUnsoldAdmin.Extensions;
using RecoverUnsoldAdmin.Services;
using RecoverUnsoldAdmin.Services.Customers;
using RecoverUnsoldAdmin.Services.Distributors;
using RecoverUnsoldAdmin.Services.Offers;
using RecoverUnsoldAdmin.Services.Stats;
using RecoverUnsoldAdmin.Utils;
using RecoverUnsoldDomain.Data;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.ConfigureDbContext(configuration);
builder.Services.ConfigureGoogleMaps(configuration);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<AuthenticationStateProvider,AppAuthenticationStateProvider>();
builder.Services.AddScoped<AppAuthenticationStateProvider>();
builder.Services.AddScoped<IDistributorsService, DistributorsService>();
builder.Services.AddScoped<ICustomersService, CustomersService>();
builder.Services.AddScoped<IOffersService, OffersService>();
builder.Services.AddScoped<IStatsService, StatsService>();


var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
}

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToPage("/_Host");
app.Run();