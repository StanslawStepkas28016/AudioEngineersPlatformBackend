using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Infrastructure.Context;
using AudioEngineersPlatformBackend.Infrastructure.ExternalServices;
using AudioEngineersPlatformBackend.Infrastructure.ExternalServices.MailService;
using AudioEngineersPlatformBackend.Infrastructure.ExternalServices.S3Service;
using AudioEngineersPlatformBackend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAdvertRepository, AdvertRepository>();

        // Add external Services
        services.AddScoped<IEmailService, EmailService>();
        services.Configure<MailtrapSettings>(
            configuration.GetSection("MailtrapSettings")
        );

        services.AddScoped<IS3Service, S3Service>();
        services.Configure<S3Settings>(
            configuration.GetSection("S3Settings")
        );

        services.AddSingleton<IAmazonS3>(sp =>
        {
            S3Settings s3Settings = sp.GetRequiredService<IOptions<S3Settings>>().Value;

            BasicAWSCredentials credentials = new BasicAWSCredentials(s3Settings.AccessKey, s3Settings.SecretKey);

            AmazonS3Config config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(s3Settings.Region)
            };

            return new AmazonS3Client(credentials, config);
        });


        // Add a unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}