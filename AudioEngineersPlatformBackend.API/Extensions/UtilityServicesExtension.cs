using API.Abstractions;
using API.Util.ClaimsUtil;
using API.Util.CookieUtil;
using API.Util.JwtUtil;
using AudioEngineersPlatformBackend.Application.Config.Settings;

namespace API.Extensions;

public static class UtilityServicesExtension
{
    public static IServiceCollection AddUtilityServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Add jwt token util.
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.AddScoped<IJwtUtil, JwtUtil>();

        // Add cookie util.
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ICookieUtil, CookieUtil>();

        // Add claims extractor util.
        services.AddScoped<IClaimsUtil, ClaimsUtil>();

        return services;
    }
}