using Microsoft.EntityFrameworkCore;
using Npgsql;
using RecoverUnsoldDomain.Config;
using RecoverUnsoldDomain.Data;

namespace RecoverUnsoldAdmin.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureGoogleMaps(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var cfg = configuration.GetSection("Maps");
        cfg["Key"] = configuration["MapsApiKey"];
        serviceCollection.Configure<MapsConfig>(cfg);
    }

    public static void ConfigureDbContext(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var connectionBuilder = new NpgsqlConnectionStringBuilder
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection"),
            Database = configuration["PgDbName"] ?? "recover_unsold",
            Host = configuration["PgHost"] ?? "127.0.0.1",
            Password = configuration["PgPassword"],
            Username = configuration["PgUserId"]
        };
        serviceCollection.AddDbContextFactory<DataContext>(
            opt => opt.UseNpgsql(
                connectionBuilder.ConnectionString,
                o => o.UseNetTopologySuite()
            )
        );
    }
}