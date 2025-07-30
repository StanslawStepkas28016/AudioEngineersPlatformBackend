using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Services;
using AudioEngineersPlatformBackend.Application.Util;
using AudioEngineersPlatformBackend.Application.Util.Cookies;
using AudioEngineersPlatformBackend.Application.Util.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AudioEngineersPlatformBackend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Add application layer services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAdvertService, AdvertService>();

        // Add settings for JWT
        services.Configure<JwtSettings>(configuration.GetSection("JWTSettings"));
        services.AddScoped<ITokenUtil, TokenUtil>();
        
        // Add HttpContextAccessor and CookieUtil for accessing cookies sent in the requests
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ICookieUtil, CookieUtil>();

        // Add services for current user
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}