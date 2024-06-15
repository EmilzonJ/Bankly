using System.Reflection;
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
    }
}
