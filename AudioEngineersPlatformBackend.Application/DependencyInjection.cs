using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Services;
using AudioEngineersPlatformBackend.Application.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AudioEngineersPlatformBackend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Add application layer services
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        // Add settings for JWT
        services.AddScoped<IJWTFactory, JWTFactory>();
        services.Configure<JWTSettings>(
            configuration.GetSection("JWTSettings")
        );

        return services;
    }
}