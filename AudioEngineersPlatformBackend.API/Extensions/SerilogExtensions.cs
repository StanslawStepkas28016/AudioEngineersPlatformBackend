using Serilog;

namespace API.Extensions;

public static class SerilogExtensions
{
    public static void AddSerilogLogging(
        this IHostBuilder hostBuilder
    )
    {
        hostBuilder.UseSerilog
        ((
                context,
                configuration
            ) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            }
        );
    }
}