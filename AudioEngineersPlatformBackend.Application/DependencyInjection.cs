using System.Reflection;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Config.Settings;
using AudioEngineersPlatformBackend.Application.Util.UrlGeneratorUtil;
using AudioEngineersPlatformBackend.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AudioEngineersPlatformBackend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Add FluentValidation.
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Add Automapper.
        services.Configure<LuckyPennySoftwareSettings>(configuration.GetSection(nameof(LuckyPennySoftwareSettings)));
        services.AddAutoMapper
        (
            (
                provider,
                cfg
            ) =>
            {
                cfg.LicenseKey = provider
                    .GetRequiredService<IOptions<LuckyPennySoftwareSettings>>()
                    .Value
                    .LicenseKey;
            },
            AppDomain.CurrentDomain.GetAssemblies()
        );

        // Add Mediator.
        services.AddMediatR
        (cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                cfg.LicenseKey = services
                    .BuildServiceProvider()
                    .GetRequiredService<IOptions<LuckyPennySoftwareSettings>>()
                    .Value
                    .LicenseKey;
            }
        );

        // Add password hasher for DI.
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        // Add url generator util.
        services.Configure<FrontendSettings>(configuration.GetSection(nameof(FrontendSettings)));
        services.AddScoped<IUrlGeneratorUtil, UrlGeneratorUtil>();

        return services;
    }
}