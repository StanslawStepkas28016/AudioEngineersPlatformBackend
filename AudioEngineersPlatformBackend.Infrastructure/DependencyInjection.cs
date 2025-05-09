using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Infrastructure.Context;
using AudioEngineersPlatformBackend.Infrastructure.ExternalServices;
using AudioEngineersPlatformBackend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AudioEngineersPlatformBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add DbContexts
        services.AddDbContext<EngineersPlatformDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("DevDB")));

        // Add Repositories
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

        // Add external Services
        services.AddScoped<IEmailService, EmailService>();
        services.Configure<MailtrapSettings>(
            configuration.GetSection("MailtrapSettings")
        );

        // Add a unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}