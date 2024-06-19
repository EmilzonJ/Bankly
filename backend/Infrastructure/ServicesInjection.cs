using System.Reflection;
using Application.Caching;
using Domain.Contracts;
using Infrastructure.Caching;
using Infrastructure.Consumers;
using Infrastructure.MongoContext;
using Infrastructure.Repositories;
using Infrastructure.Settings;
using MassTransit;
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
        });
    }
}
