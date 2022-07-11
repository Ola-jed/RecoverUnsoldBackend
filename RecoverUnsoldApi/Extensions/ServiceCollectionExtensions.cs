using System.Text;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using RecoverUnsoldApi.Config;
using RecoverUnsoldApi.Data;
using RecoverUnsoldApi.Infrastructure;
using RecoverUnsoldApi.Services.ApplicationUser;
using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldApi.Services.ForgotPassword;
using RecoverUnsoldApi.Services.Locations;
using RecoverUnsoldApi.Services.Mail;
using RecoverUnsoldApi.Services.Offers;
using RecoverUnsoldApi.Services.Products;
using RecoverUnsoldApi.Services.UserVerification;

namespace RecoverUnsoldApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigurePgsql(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection"),
            Database = configuration["PgDbName"] ?? "recover_unsold",
            Host = configuration["PgHost"] ?? "127.0.0.1",
            Password = configuration["PgPassword"],
            Username = configuration["PgUserId"]
        };
        serviceCollection.AddDbContext<DataContext>(opt =>
            opt.UseNpgsql(builder.ConnectionString, o => o.UseNetTopologySuite()));
    }

    public static void ConfigureAuthentication(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
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

    public static void ConfigureCloudinary(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var account = new Account(
            configuration["CloudinaryCloud"],
            configuration["CloudinaryApiKey"],
            configuration["CloudinaryApiSecret"]
        );
        serviceCollection.AddSingleton(new Cloudinary(account));
    }

    public static void ConfigureSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(c =>
        {
            c.UseAllOfForInheritance();
            c.UseOneOfForPolymorphism();
            c.SelectSubTypesUsing(baseType =>
                typeof(Program).Assembly.GetTypes().Where(type => type.IsSubclassOf(baseType))
            );
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "RecoverUnsold",
                Version = "v1",
                Description = "Api pour une application d'écoulement d'invendus",
                Contact = new OpenApiContact
                {
                    Name = "Olabissi Gbangboche",
                    Email = "olabijed@gmail.com",
                    Url = new Uri("https://github.com/Ola-jed")
                }
            });
            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "Authorization header using the Bearer scheme. (\"Authorization: Bearer {token}\")",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            c.AddSecurityDefinition("Bearer", securitySchema);
            var securityRequirement = new OpenApiSecurityRequirement
            {
                { securitySchema, new[] { "Bearer" } }
            };
            c.AddSecurityRequirement(securityRequirement);
        });
    }

    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<LongRunningService>();
        serviceCollection.AddSingleton<BackgroundWorkerQueue>();
        serviceCollection.AddScoped<IAuthService, AuthService>();
        serviceCollection.AddScoped<IApplicationUserService, ApplicationUserService>();
        serviceCollection.AddScoped<IUserVerificationService, UserVerificationService>();
        serviceCollection.AddScoped<IForgotPasswordService, ForgotPasswordService>();
        serviceCollection.AddScoped<IMailService, MailService>();
        serviceCollection.AddScoped<ILocationsService, LocationsService>();
        serviceCollection.AddScoped<IProductsService, ProductsService>();
        serviceCollection.AddScoped<IOffersService, OffersService>();
    }
}