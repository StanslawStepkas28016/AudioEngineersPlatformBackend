namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IEmailService
{
    Task TrySendRegisterVerificationEmailAsync(string toEmail, string firstName, string? verificationCode);
}