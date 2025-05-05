using API.Middlewares.Exception;
using AudioEngineersPlatformBackend.Application;
using AudioEngineersPlatformBackend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Clean Architecture layers
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();

// Add all controllers (those who inherit ControllerBase class)
builder.Services.AddControllers();

var app = builder.Build();

// Use custom middlewares
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();