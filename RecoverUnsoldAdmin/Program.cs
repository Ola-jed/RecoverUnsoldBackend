using Blazored.LocalStorage;
using MudBlazor.Services;
using RecoverUnsoldAdmin.Services;
using RecoverUnsoldDomain.Extensions;
using RecoverUnsoldDomain.Services.Distributors;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<IDistributorsService, DistributorsService>();
builder.Services.ConfigurePgsql(configuration);

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