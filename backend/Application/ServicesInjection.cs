using System.Reflection;
using Application.Features.Transactions.Contracts;
using Application.Features.Transactions.Factories;
using Application.Features.Transactions.Strategies;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServicesInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        Assembly currentAssembly = Assembly.LoadFrom(typeof(ServicesInjection).Assembly.Location);
        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Transient);
        services.AddValidatorsFromAssembly(currentAssembly);

        services.AddScoped<DepositStrategy>();
        services.AddScoped<WithdrawalStrategy>();
        services.AddScoped<TransferStrategy>();
        services.AddScoped<ITransactionStrategyFactory, TransactionStrategyFactory>();
    }
}
