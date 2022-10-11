using System.Text.Json.Serialization;
using RecoverUnsoldApi.Converters;
using RecoverUnsoldApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Services.ConfigurePgsql(configuration);
builder.Services.ConfigureFirebase(configuration);
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    x.JsonSerializerOptions.Converters.Add(new JsonDictionaryConverter<DateTime, int>());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureMail(configuration);
builder.Services.ConfigureAppOwner(configuration);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCloudinary(configuration);
builder.Services.AddServices();
builder.Services.ConfigureAuthentication(configuration);

var app = builder.Build();
if (app.Environment.IsProduction())
{
    var port = Environment.GetEnvironmentVariable("PORT");
    app.Urls.Add($"http://*:{port}");
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();