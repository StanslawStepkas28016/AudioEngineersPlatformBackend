using System.Threading;
using System.Threading.Tasks;
using AudioEngineersPlatformBackend.Dtos.Auth.Utilities;

namespace AudioEngineersPlatformBackend.Services.EmailService;

public interface IEmailService
{
    public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken);

    public Task SendVerificationEmailAsync(VerifyEmailUserDataDto verifyEmailUserDataDto,
        CancellationToken cancellationToken);
}