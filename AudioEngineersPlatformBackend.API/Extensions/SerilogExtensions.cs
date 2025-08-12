using Serilog;
using Serilog.Formatting.Json;

namespace API.Extensions;

public static class SerilogExtensions
{
    public static void AddSerilogLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureLogging
            ((_, builder) => { builder.ClearProviders(); });

        hostBuilder.UseSerilog
        ((_, configuration) =>
            {
                // Configure writing to console.
                configuration.WriteTo.Console();

                // Configure writing to a separate file by specifying the rollingInterval.
                // configuration.WriteTo.File
                //     (new JsonFormatter(), "Logs/log.txt", rollingInterval: RollingInterval.Day);
            }
        );
    }
}