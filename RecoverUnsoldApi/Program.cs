using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using RecoverUnsoldApi.Converters;
using RecoverUnsoldApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.WebHost.UseSentry(o =>
{
    o.Dsn = configuration["SentryDsn"];
    o.TracesSampleRate = 1.0;
});
builder.Services.ConfigurePgsql(configuration);
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    x.JsonSerializerOptions.Converters.Add(new JsonDictionaryConverter<DateTime, int>());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureRabbitmq(configuration);
builder.Services.ConfigureAppOwner(configuration);
builder.Services.AddHttpClient("Kkiapay", httpClient =>
{
    httpClient.BaseAddress = new Uri(configuration["KkiapayBaseUrl"]!);
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    httpClient.DefaultRequestHeaders.Add("X-API-KEY", configuration["KkiapayPublicKey"]);
    httpClient.DefaultRequestHeaders.Add("X-PRIVATE-KEY", configuration["KkiapayPrivateKey"]);
    httpClient.DefaultRequestHeaders.Add("X-SECRET-KEY", configuration["KkiapaySecret"]);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    var certStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RecoverUnsoldApi.Resource.cacert.pem")!;
    var cert = X509Certificate2.CreateFromPem(new StreamReader(certStream).ReadToEnd());
    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
    handler.ClientCertificates.Add(cert);
    return handler;
});
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCloudinary(configuration);
builder.Services.AddServices();
builder.Services.ConfigureAuthentication(configuration);

var app = builder.Build();
app.UseStaticFiles();
app.UseSwagger();
var currentAssembly = Assembly.GetAssembly(typeof(Program))!;
app.UseSwaggerUI(c =>
{
    c.DocumentTitle = "RecoverUnsold Api";
    c.IndexStream = () =>
        currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.wwwroot.swagger-index.html");
    c.InjectStylesheet("/swagger-theme-material.css");
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();