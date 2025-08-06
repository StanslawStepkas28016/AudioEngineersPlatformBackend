using AudioEngineersPlatformBackend.Application.Util.UrlGenerator;

namespace API.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                FrontendSettings frontendSettings =
                    configuration.GetSection("FrontendSettings").Get<FrontendSettings>()!;

                policy
                    .WithOrigins(frontendSettings.Url)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}