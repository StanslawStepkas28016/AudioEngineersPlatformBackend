using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace API.Extensions;

public static class LocalizationExtension
{
    public static IServiceCollection AddLocalizationExtension(this IServiceCollection services, IConfiguration configuration)
    {
        // The path is not specified, as it will be specified in a different assembly!
        // This is caused by the fact that each assembly is responsible for different things in Clean Architecture
        services.AddLocalization();

        services.Configure<RequestLocalizationOptions>(opts =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("pl-pl"),
                new CultureInfo("en-us")
            };

            opts.DefaultRequestCulture = new RequestCulture("en", "en");
            opts.SupportedCultures = supportedCultures;
            opts.SupportedUICultures = supportedCultures;
        });

        return services;
    }
}