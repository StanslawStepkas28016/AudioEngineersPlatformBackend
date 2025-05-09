using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Services;

namespace AudioEngineersPlatformBackend.Application;

using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        // Use case internal services
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        
        return services;
    }
}