using Npgsql;
using OpenTelemetry;
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
        services
            .AddOpenTelemetry()
            .ConfigureResource
            (resource =>
                resource
                    .AddService(nameof(AudioEngineersPlatformBackend))
            )
            .WithTracing
            (tracing => tracing
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddNpgsql()
            )
            .WithMetrics
            (metrics => metrics
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
            )
            .WithLogging()
            .UseOtlpExporter();

        return services;
    }
}