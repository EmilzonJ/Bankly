using Web.Config;

namespace Web;

public static class DependencyInjection
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

    private static void AddSwaggerService(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    private static void AddCorsService(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(StaticSetting.CorsPolicyName,
                b => b
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );
        });
    }
}
