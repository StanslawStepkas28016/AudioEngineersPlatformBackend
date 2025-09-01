using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Infrastructure.Context;
using AudioEngineersPlatformBackend.Infrastructure.ExternalServices.S3Service;
using AudioEngineersPlatformBackend.Infrastructure.ExternalServices.SESService;
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
        services.AddDbContext<EngineersPlatformDbContext>
        (options => options
            .UseSqlServer(configuration.GetConnectionString("DevDB"))
        );

        // Add Repositories
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAdvertRepository, AdvertRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();

        // Add AWS SES
        services.Configure<SESSettings>(configuration.GetSection(nameof(SESSettings)));
        services.AddScoped<ISESService, SESService>();
        services.AddSingleton<IAmazonSimpleEmailService>
        (sp =>
            {
                SESSettings sesSettings = sp.GetRequiredService<IOptions<SESSettings>>().Value;

                BasicAWSCredentials credentials = new BasicAWSCredentials(sesSettings.AccessKey, sesSettings.SecretKey);

                AmazonSimpleEmailServiceConfig config = new AmazonSimpleEmailServiceConfig()
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(sesSettings.Region)
                };

                return new AmazonSimpleEmailServiceClient(credentials, config);
            }
        );

        // Add AWS S3
        services.Configure<S3Settings>(configuration.GetSection(nameof(S3Settings)));
        services.AddScoped<IS3Service, S3Service>();
        services.AddSingleton<IAmazonS3>
        (sp =>
            {
                S3Settings s3Settings = sp.GetRequiredService<IOptions<S3Settings>>().Value;

                BasicAWSCredentials credentials = new BasicAWSCredentials(s3Settings.AccessKey, s3Settings.SecretKey);

                AmazonS3Config config = new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(s3Settings.Region)
                };

                return new AmazonS3Client(credentials, config);
            }
        );

        // Add a unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}