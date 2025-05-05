using Microsoft.Extensions.DependencyInjection;

namespace AudioEngineersPlatformBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        // services.AddScoped<IEmailService, EmailService>();
        // services.AddScoped<IAppointmentManagerService, AppointmentManagerService>();
        // services.AddScoped<IDbContext, GakkoContext>();
        return services;
    }
}