using API.Extensions;
using API.Middlewares.ExceptionMiddleware;
using AudioEngineersPlatformBackend.Application;
using AudioEngineersPlatformBackend.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    // Add Swagger fore development
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add localization
    builder.Services.AddLocalizationExtension(builder.Configuration);

    // Add "Clean Architecture layers"
    builder.Services.AddApplicationLayer(builder.Configuration);
    builder.Services.AddInfrastructureLayer(builder.Configuration);

    // Add all controllers (those which inherit from ControllerBase class)
    builder.Services.AddControllers().AddNewtonsoftJson();

    // Add authentication and authorization
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddRoleAuthorization();

    // Add support for CORS
    builder.Services.AddCorsPolicy(builder.Configuration);

    // TODO: Add a logger.
    // builder.Logging.ClearProviders(); 
}

WebApplication app = builder.Build();
{
    // Use custom middlewares
    app.UseMiddleware<ExceptionMiddleware>();

    // Use swagger for development
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Use redirections from HTTP to HTTPS
    app.UseHttpsRedirection();

    // User routing an CORS 
    app.UseRouting();
    app.UseCors();

    // Use authentication and authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // Use request localization
    app.UseRequestLocalization();

    // Map controllers to endpoints
    app.MapControllers();

    app.Run();
}