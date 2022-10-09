using Blazored.LocalStorage;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Npgsql;
using RecoverUnsoldAdmin.Services;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldAdmin.Services.Distributors;
using RecoverUnsoldAdmin.Services.Stats;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var connectionBuilder = new NpgsqlConnectionStringBuilder
{
    ConnectionString = configuration.GetConnectionString("DefaultConnection"),
    Database = configuration["PgDbName"] ?? "recover_unsold",
    Host = configuration["PgHost"] ?? "127.0.0.1",
    Password = configuration["PgPassword"],
    Username = configuration["PgUserId"]
};
builder.Services.AddDbContextFactory<DataContext>(
    opt => opt.UseNpgsql(connectionBuilder.ConnectionString,
        o => o.UseNetTopologySuite())
);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<IDistributorsService, DistributorsService>();
builder.Services.AddScoped<IStatsService, StatsService>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();