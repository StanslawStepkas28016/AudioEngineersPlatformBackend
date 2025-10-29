using AudioEngineersPlatformBackend.Application.Config;
using AudioEngineersPlatformBackend.Application.Config.Settings;
using Microsoft.Extensions.Options;

namespace API.Extensions;

public static class CorsExtension
{
    public static IServiceCollection AddCorsPolicy(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<FrontendSettings>(configuration.GetSection(nameof(FrontendSettings)));

        services.AddCors
        (options =>
            {
                options.AddDefaultPolicy
                (policy =>
                    {
                        FrontendSettings frontendSettings = services
                            .BuildServiceProvider()
                            .GetRequiredService<IOptions<FrontendSettings>>()
                            .Value;

                        policy
                            .WithOrigins(frontendSettings.Url)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    }
                );
            }
        );

        return services;
    }
}