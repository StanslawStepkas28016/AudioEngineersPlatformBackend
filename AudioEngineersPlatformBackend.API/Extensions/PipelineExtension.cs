using API.Middlewares;
using Microsoft.Extensions.Options;
using Serilog;

namespace API.Extensions;

public static class PipelineExtension
{
    public static IApplicationBuilder ConfigurePipeline(
        this IApplicationBuilder app,
        IWebHostEnvironment environment
    )
    {
        // Use custom middlewares.
        app.UseMiddleware<ExceptionMiddleware>();

        // Use swagger for development.
        if (environment.IsDevelopment() || environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Use request logging middleware.
        app.UseSerilogRequestLogging();

        // Use redirections from HTTP to HTTPS.
        app.UseHttpsRedirection();

        // User routing an CORS.
        app.UseRouting();
        app.UseCors();

        // Use authentication and authorization.
        app.UseAuthentication();
        app.UseAuthorization();

        // Use request localization.
        IOptions<RequestLocalizationOptions> requestLocalizationOptions =
            app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();

        app.UseRequestLocalization(requestLocalizationOptions.Value);

        return app;
    }
}