using Web.Infrastructure;
using Web.Settings;

namespace Web;

public static class ServicesInjection
{
    public static void AddWebServices(this IServiceCollection services)
    {
        services.AddDefaultServices();
        services.AddSwaggerService();
        services.AddCorsService();
    }

    private static void AddDefaultServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            }
        );
    }

    private static void AddExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }

    private static void AddSwaggerService(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    private static void AddCorsService(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(StaticSettings.CorsPolicyName,
                b => b
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );
        });
    }
}
