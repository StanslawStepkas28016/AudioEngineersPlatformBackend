using System.Globalization;
using AudioEngineersPlatformBackend.Middlewares;
using AudioEngineersPlatformBackend.Models;
using AudioEngineersPlatformBackend.Repositories.AuthRepository;
using AudioEngineersPlatformBackend.Services.AuthService;
using AudioEngineersPlatformBackend.Services.EmailService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AudioEngineersPlatformBackend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();

        // Repositories
        builder.Services.AddScoped<IAuthRepository, AuthRepository>();

        // Services
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.Configure<AuthService.AuthVariables>(
            builder.Configuration.GetSection("AuthVariables")
        );

        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.Configure<EmailService.SmtpSettings>(
            builder.Configuration.GetSection("SmtpSettings")
        );

        // Context
        builder.Services.AddDbContext<MasterContext>(opt =>
        {
            var connectionString = builder
                .Configuration
                .GetConnectionString("DevelopmentDB");
            opt.UseSqlServer(connectionString);
        });
        
        // Add Localization
        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();

        // Middleware
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseMiddleware<LocalizationMiddleware>();
        
        // Setup for Localization
        var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("pl-PL") };
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en-US"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        };
        app.UseRequestLocalization(localizationOptions);

        app.MapControllers();

        app.Run();
    }
}