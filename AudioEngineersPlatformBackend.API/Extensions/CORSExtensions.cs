namespace API.Extensions;

public static class CORSExtensions
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins(configuration.GetSection("FrontendSettingsDev:URL").Value!)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });

            /*options.Add(configuration.GetSection("FrontendSettings:PolicyName").Value!, policy =>
            {
                policy
                    .WithOrigins(configuration.GetSection("FrontendSettings:URL").Value!)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });*/
        });

        return services;
    }
}