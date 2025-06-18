
namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface ISESService
{
    Task SendRegisterVerificationEmailAsync(string toEmail, string firstName,
        string? verificationCode);

    Task SendEmailResetEmailAsync(string toEmail, string firstName, string uniqueUrl);
    Task SendPasswordResetEmailAsync(string toEmail, string firstName, string uniqueUrl);
}