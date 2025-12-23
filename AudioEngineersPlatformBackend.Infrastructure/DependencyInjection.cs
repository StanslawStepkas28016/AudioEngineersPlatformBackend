using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Infrastructure.Config.Settings;
using AudioEngineersPlatformBackend.Infrastructure.External.S3Service;
using AudioEngineersPlatformBackend.Infrastructure.External.SesService;
using AudioEngineersPlatformBackend.Infrastructure.Persistence.Context;
using AudioEngineersPlatformBackend.Infrastructure.Repositories;
using AudioEngineersPlatformBackend.Infrastructure.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AudioEngineersPlatformBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Add DbContext.
        services.AddDbContext<AudioEngineersPlatformDbContext>
        (builder => builder
            .UseNpgsql(configuration.GetConnectionString("DevDB"))
        );

        // Add Repositories.
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAdvertRepository, AdvertRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();

        // Add AWS SES.
        services.Configure<AwsSettings>(configuration.GetSection(nameof(AwsSettings)));
        services.AddSingleton<IAmazonSimpleEmailService>
        (sp =>
            {
                AwsSettings sesSettings = sp.GetRequiredService<IOptions<AwsSettings>>()
                    .Value;

                BasicAWSCredentials credentials = new BasicAWSCredentials(sesSettings.AccessKey, sesSettings.SecretKey);

                AmazonSimpleEmailServiceConfig config = new AmazonSimpleEmailServiceConfig
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(sesSettings.Region)
                };

                return new AmazonSimpleEmailServiceClient(credentials, config);
            }
        );
        services.AddScoped<ISesService, SesService>();

        // Add AWS S3.
        services.Configure<AwsSettings>(configuration.GetSection(nameof(AwsSettings)));
        services.AddSingleton<IAmazonS3>
        (sp =>
            {
                AwsSettings awsSettings = sp.GetRequiredService<IOptions<AwsSettings>>()
                    .Value;

                BasicAWSCredentials credentials = new BasicAWSCredentials(awsSettings.AccessKey, awsSettings.SecretKey);

                AmazonS3Config config = new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(awsSettings.Region)
                };

                return new AmazonS3Client(credentials, config);
            }
        );
        services.AddScoped<IS3Service, S3Service>();

        // Add a UoW.
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}