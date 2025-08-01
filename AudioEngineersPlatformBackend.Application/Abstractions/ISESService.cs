
namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface ISESService
{
    Task TrySendRegisterVerificationEmailAsync(string toEmail, string firstName,
        string? verificationCode);

    Task TrySendEmailResetEmailAsync(string toEmail, string firstName, string uniqueUrl);
}