using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using RecoverUnsoldDomain.Config;

namespace RecoverUnsoldWorker.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureFirebase(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromJson(configuration["FirebaseCredential"])
        });
    }

    public static void ConfigureMail(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var cfg = configuration.GetSection("MailSettings");
        cfg["Host"] = configuration["MailHost"];
        cfg["Port"] = configuration["MailPort"];
        cfg["MailUser"] = configuration["MailUser"];
        cfg["MailDisplayName"] = configuration["MailDisplayName"];
        cfg["MailPassword"] = configuration["MailPassword"];
        serviceCollection.Configure<MailConfig>(cfg);
    }

    public static void ConfigureRabbitmq(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var cfg = configuration.GetSection("Rabbitmq");
        cfg["Uri"] = configuration["RabbitmqUri"]!;
        serviceCollection.Configure<RabbitmqConfig>(cfg);
    }
}