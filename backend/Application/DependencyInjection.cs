using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        Assembly currentAssembly = Assembly.LoadFrom(typeof(DependencyInjection).Assembly.Location);
        services.AddMediator();
        services.AddValidatorsFromAssembly(currentAssembly);
    }
}
