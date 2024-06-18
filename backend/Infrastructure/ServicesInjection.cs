using System.Reflection;
using Application.Caching;
using Domain.Contracts;
using Infrastructure.Caching;
using Infrastructure.MongoContext;
using Infrastructure.Repositories;
using Infrastructure.Settings;
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
            var connection = configuration.GetConnectionString("Redis");
            redisOptions.Configuration = connection;
        });
        services.AddSingleton<ICacheService, CacheService>();
    }
}
