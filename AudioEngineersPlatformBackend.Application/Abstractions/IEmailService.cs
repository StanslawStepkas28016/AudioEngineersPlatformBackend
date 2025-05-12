namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IEmailService
{
    Task TrySendVerificationEmailAsync(string toEmail, string firstName, string? verificationCode);
}