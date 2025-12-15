using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace API.Extensions;

public static class LocalizationExtension
{
    public static IServiceCollection AddLocalizationExtension(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // The resource bundle file path is not specified, as it will be specified
        // in a different assembly while using the IStringLocalizer!
        services.AddLocalization();

        services.Configure<RequestLocalizationOptions>
        (opts =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new("en-US"),
                    new("pl-PL")
                };

                opts.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            }
        );

        return services;
    }
}