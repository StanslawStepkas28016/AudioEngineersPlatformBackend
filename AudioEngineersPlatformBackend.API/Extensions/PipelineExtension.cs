using API.Middlewares;
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
        if (environment.IsDevelopment())
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
        app.UseRequestLocalization();

        return app;
    }
}