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
        services.Configure<SesSettings>(configuration.GetSection(nameof(SesSettings)));
        services.AddSingleton<IAmazonSimpleEmailService>
        (sp =>
            {
                SesSettings sesSettings = sp.GetRequiredService<IOptions<SesSettings>>()
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
        services.Configure<S3Settings>(configuration.GetSection(nameof(S3Settings)));
        services.AddSingleton<IAmazonS3>
        (sp =>
            {
                S3Settings s3Settings = sp.GetRequiredService<IOptions<S3Settings>>()
                    .Value;

                BasicAWSCredentials credentials = new BasicAWSCredentials(s3Settings.AccessKey, s3Settings.SecretKey);

                AmazonS3Config config = new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(s3Settings.Region)
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