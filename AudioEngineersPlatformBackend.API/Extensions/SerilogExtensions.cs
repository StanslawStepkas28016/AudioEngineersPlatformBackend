using Serilog;
using Serilog.Events;

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

                // Disable logging EF.Core 
                configuration.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning);

                // Configure writing to a separate file by specifying the rollingInterval.
                // configuration.WriteTo.File
                //     (new JsonFormatter(), "Logs/log.txt", rollingInterval: RollingInterval.Day);
            }
        );
    }
}