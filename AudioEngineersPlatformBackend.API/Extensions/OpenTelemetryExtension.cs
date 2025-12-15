using API.Config.Settings;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace API.Extensions;

public static class OpenTelemetryExtension
{
    public static IServiceCollection AddOpenTelemetryForAspire(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Configure Aspire endpoint.
        services.Configure<OltpSettings>(configuration.GetSection(nameof(OltpSettings)));

        OltpSettings oltpSettings = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<OltpSettings>>()
            .Value;

        Uri openTelemetryUri = new Uri(oltpSettings.OltpUri);
        services
            .AddOpenTelemetry()
            .ConfigureResource
            (res => res
                .AddService(nameof(AudioEngineersPlatformBackend))
            )
            .WithMetrics
            (metrics =>
                {
                    metrics
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddRuntimeInstrumentation();

                    metrics.AddOtlpExporter(opt => opt.Endpoint = openTelemetryUri);
                }
            )
            .WithTracing
            (tracing =>
                {
                    tracing
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation();
                        // .AddEntityFrameworkCoreInstrumentation();
                        // TODO: Check this for prod.

                    tracing.AddOtlpExporter(opt => opt.Endpoint = openTelemetryUri);
                }
            )
            .WithLogging
            (log =>
                {
                    log.AddOtlpExporter
                    (opt =>
                        {
                            opt.Endpoint = openTelemetryUri;
                            opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                        }
                    );
                }
            );

        return services;
    }
}