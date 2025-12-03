using System.Text;
using API.Config.Settings;
using API.Util.CookieUtil;
using AudioEngineersPlatformBackend.Application.Config.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class AuthExtension
{
    public const string AdministratorRole = "Administrator";
    public const string ClientRole = "Client";
    public const string AudioEngineerRole = "Audio engineer";

    public static IServiceCollection AddSymmetricAuth(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Add symmetric authentication schema.
        services
            .AddAuthentication
            (options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer
            (options =>
                {
                    JwtSettings jwtSettings = services
                        .BuildServiceProvider()
                        .GetRequiredService<IOptions<JwtSettings>>()
                        .Value;

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // Only pull accessToken from the cookies if it exists
                            context.Token = context.Request.Cookies[nameof(CookieNames.AccessToken)];
                            return Task.CompletedTask;
                        }
                    };

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudiences = jwtSettings.Audiences,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                    };
                }
            );

        services
            .AddAuthorizationBuilder()
            .AddPolicy
            (
                "AdministratorOnly",
                p =>
                    p.RequireRole(AdministratorRole)
            )
            .AddPolicy
            (
                "Everyone",
                p =>
                    p.RequireRole(AdministratorRole, ClientRole, AudioEngineerRole)
            );

        return services;
    }
}