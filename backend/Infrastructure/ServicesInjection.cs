using System.Reflection;
using System.Text;
using Application.Caching;
using Application.Features.Auth.Contracts;
using Domain.Contracts;
using Infrastructure.Authentication;
using Infrastructure.Caching;
using Infrastructure.Consumers;
using Infrastructure.MongoContext;
using Infrastructure.Repositories;
using Infrastructure.Settings;
using Infrastructure.SettingsSetup;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Infrastructure;

public static class ServicesInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDb(configuration);
        services.AddRepositories();
        services.AddCaching(configuration);
        services.AddRabbitMq(configuration);
        services.AddJwtAuthentication();
    }

    private static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
        services.AddSingleton<MongoDbContext>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        // Decorated Repositories
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.Decorate<ITransactionRepository, TransactionRepositoryCached>();

        services.Scan(selector => selector
            .FromAssemblies(Assembly.LoadFrom(typeof(ServicesInjection).Assembly.Location))
            .AddClasses()
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface()
            .WithScopedLifetime()
        );
    }

    private static void AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            var connection = configuration["Redis:Url"];
            redisOptions.Configuration = connection;
        });
        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
                {
                    h.Username(configuration["MessageBroker:Username"]!);
                    h.Password(configuration["MessageBroker:Password"]!);
                });

                configurator.ConfigureEndpoints(context);
            });

            busConfigurator.AddConsumer<CustomerNameUpdatedConsumer>();
            busConfigurator.AddConsumer<CustomerEmailUpdatedConsumer>();
            busConfigurator.AddConsumer<CustomerDeletedConsumer>();
        });
    }

    private static void AddJwtAuthentication(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtSettingsSetup>();
        services.ConfigureOptions<JwtBearerSettingsSetup>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
    }
}
