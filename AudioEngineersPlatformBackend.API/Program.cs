using API.Extensions;
using API.Hubs;
using AudioEngineersPlatformBackend.Application;
using AudioEngineersPlatformBackend.Infrastructure;
using AudioEngineersPlatformBackend.Infrastructure.Persistence.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add logging via Serilog.
builder.Host.AddSerilogLogging();

// Add Swagger for development.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add localization.
builder.Services.AddLocalizationExtension(builder.Configuration);

// Add "Clean Architecture layers".
builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer(builder.Configuration);

// Add SignalR.
builder.Services.AddSignalR();

// Add controllers.
builder.Services.AddControllers();

// Add authentication and authorization.
builder.Services.AddSymmetricAuth(builder.Configuration);

// Add support for CORS.
builder.Services.AddCorsPolicy(builder.Configuration);

// Add utility services.
builder.Services.AddUtilityServices(builder.Configuration);

// Add open telemetry.
builder.Services.AddOpenTelemetryForAspire(builder.Configuration);

WebApplication app = builder.Build();

app.ConfigurePipeline(app.Environment);

// Ensure running migrations.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    using IServiceScope scope = app.Services.CreateScope();

    AudioEngineersPlatformDbContext dbContext =
        scope.ServiceProvider.GetRequiredService<AudioEngineersPlatformDbContext>();

    await dbContext.Database.MigrateAsync();
}

// Ensure valid configurations.
IMapper mapper = app.Services.GetRequiredService<IMapper>();
mapper.ConfigurationProvider.AssertConfigurationIsValid();

// Map controllers to endpoints.
app.MapControllers();

// Map the chat-hub.
app.MapHub<ChatHub>("chat-hub");

app.Run();