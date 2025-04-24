using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using AudioEngineersPlatformBackend.Dtos.Auth.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace AudioEngineersPlatformBackend.Services.EmailService;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    private readonly IWebHostEnvironment _env;

    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
    }

    public EmailService(IOptions<SmtpSettings> smtpSettings, IWebHostEnvironment env)
    {
        _smtpSettings = smtpSettings.Value;
        _env = env;
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken)
    {
        using var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
        {
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = true
        };

        var message = new MailMessage(_smtpSettings.From, to, subject, body) { IsBodyHtml = true };

        await smtpClient.SendMailAsync(message, cancellationToken);
    }

    public async Task SendVerificationEmailAsync(VerifyEmailUserDataDto verifyEmailUserDataDto,
        CancellationToken cancellationToken)
    {
        var locale = CultureInfo.CurrentCulture.Name;
        await Console.Out.WriteLineAsync(locale + "LOCALE!!!!");
        var path = Path.Combine(_env.ContentRootPath, "EmailTemplates" + "/" + locale, "VerificationEmail.html");
        var template = await File.ReadAllTextAsync(path, cancellationToken);
        var preparedTemplate = template.Replace("{userName}", verifyEmailUserDataDto.Username)
            .Replace("{verificationCode}", verifyEmailUserDataDto.VerificationCode);

        await SendEmailAsync(verifyEmailUserDataDto.Email, "Please verify your account!", preparedTemplate,
            cancellationToken);
    }

    public async Task SendWelcomeEmailAsync(WelcomeEmailUserDataDto welcomeEmailUserDataDto,
        CancellationToken cancellationToken)
    {
        var path = Path.Combine(_env.ContentRootPath, "Templates", "WelcomeEmail.html");
        var template = await File.ReadAllTextAsync(path, cancellationToken);
        var preparedTemplate = template.Replace("{userName}", welcomeEmailUserDataDto.Username)
            .Replace("{firstName}", welcomeEmailUserDataDto.FirstName)
            .Replace("{lastName}", welcomeEmailUserDataDto.LastName);

        await SendEmailAsync(welcomeEmailUserDataDto.Email, "Thank you for registering and verifying!",
            preparedTemplate,
            cancellationToken);
    }
}