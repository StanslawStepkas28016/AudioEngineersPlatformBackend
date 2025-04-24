using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Middlewares;

// Middleware used for retrieving the Accept-Language header contents from requests,
// retrievable via: var localeFromCulture = CultureInfo.CurrentCulture.Name;
public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var acceptLang = context.Request.Headers.AcceptLanguage.ToString();

        if (string.IsNullOrWhiteSpace(acceptLang))
        {
            acceptLang = "en-US";
        }

        var cultureName = acceptLang.Split(',').FirstOrDefault();
        if (!string.IsNullOrEmpty(cultureName))
        {
            var culture = new CultureInfo(cultureName);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        await _next(context);
    }
}