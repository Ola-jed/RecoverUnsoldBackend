using System.Text;
using CloudinaryDotNet;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using RecoverUnsoldApi.Infrastructure;
using RecoverUnsoldApi.Services.Alerts;
using RecoverUnsoldApi.Services.ApplicationUser;
using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldApi.Services.Distributors;
using RecoverUnsoldApi.Services.FcmTokens;
using RecoverUnsoldApi.Services.ForgotPassword;
using RecoverUnsoldApi.Services.Home;
using RecoverUnsoldApi.Services.Locations;
using RecoverUnsoldApi.Services.Mail;
using RecoverUnsoldApi.Services.Notification;
using RecoverUnsoldApi.Services.Notification.OfferPublishedNotification;
using RecoverUnsoldApi.Services.Offers;
using RecoverUnsoldApi.Services.Opinions;
using RecoverUnsoldApi.Services.Orders;
using RecoverUnsoldApi.Services.Payments;
using RecoverUnsoldApi.Services.Products;
using RecoverUnsoldApi.Services.Reviews;
using RecoverUnsoldApi.Services.UserVerification;
using RecoverUnsoldDomain.Config;
using RecoverUnsoldDomain.Data;

namespace RecoverUnsoldApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigurePgsql(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionBuilder = new NpgsqlConnectionStringBuilder
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection"),
            Database = configuration["PgDbName"] ?? "recover_unsold",
            Host = configuration["PgHost"] ?? "127.0.0.1",
            Password = configuration["PgPassword"],
            Username = configuration["PgUserId"]
        };
        serviceCollection.AddDbContext<DataContext>(
            opt => opt.UseNpgsql(connectionBuilder.ConnectionString,
                o => o.UseNetTopologySuite())
        );
    }

    public static void ConfigureAuthentication(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!))
                };
            });
    }

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

    public static void ConfigureAppOwner(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var cfg = configuration.GetSection("AppOwner");
        cfg["Name"] = configuration["AppOwnerName"];
        cfg["Email"] = configuration["AppOwnerEmail"];
        serviceCollection.Configure<AppOwner>(cfg);
    }

    public static void ConfigureCloudinary(this IServiceCollection serviceCollection, IConfiguration configuration)
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
        serviceCollection.AddSingleton<IMailService, MailService>();
        serviceCollection.AddSingleton<INotificationService, NotificationService>();
        serviceCollection.AddSingleton<IOfferPublishedNotificationService, OfferPublishedNotificationService>();
        serviceCollection.AddScoped<IAuthService, AuthService>();
        serviceCollection.AddScoped<IApplicationUserService, ApplicationUserService>();
        serviceCollection.AddScoped<IUserVerificationService, UserVerificationService>();
        serviceCollection.AddScoped<IForgotPasswordService, ForgotPasswordService>();
        serviceCollection.AddScoped<ILocationsService, LocationsService>();
        serviceCollection.AddScoped<IProductsService, ProductsService>();
        serviceCollection.AddScoped<IOffersService, OffersService>();
        serviceCollection.AddScoped<IOrdersService, OrdersService>();
        serviceCollection.AddScoped<IDistributorsService, DistributorsService>();
        serviceCollection.AddScoped<IFcmTokensService, FcmTokensService>();
        serviceCollection.AddScoped<IReviewsService, ReviewsService>();
        serviceCollection.AddScoped<IAlertsService, AlertsService>();
        serviceCollection.AddScoped<IOpinionsService, OpinionsService>();
        serviceCollection.AddScoped<IHomeService, HomeService>();
        serviceCollection.AddScoped<IPaymentsService, PaymentsService>();
    }
}