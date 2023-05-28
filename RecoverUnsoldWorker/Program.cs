using RecoverUnsoldWorker;
using RecoverUnsoldWorker.Extensions;
using RecoverUnsoldWorker.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.ConfigureRabbitmq(context.Configuration);
        services.ConfigureMail(context.Configuration);
        services.ConfigureFirebase(context.Configuration);
        services.AddHostedService<MailWorker>();
        services.AddHostedService<FirebaseWorker>();
    })
    .Build();

host.Run();