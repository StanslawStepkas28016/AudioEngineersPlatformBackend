using AudioEngineersPlatformBackend.Application.Abstractions;
using Microsoft.Extensions.Options;

namespace AudioEngineersPlatformBackend.Application.Util.UrlGenerator;

public class UrlGeneratorUtil : IUrlGeneratorUtil
{
    public FrontendSettings FrontendSettings { get; set; }

    public UrlGeneratorUtil(IOptions<FrontendSettings> frontendSettings)
    {
        FrontendSettings = frontendSettings.Value;
    }

    public Task<string> GenerateResetEmailUrl(Guid emailResetToken)
    {
        return Task.FromResult($"{FrontendSettings.Url}/{emailResetToken}/verify-reset-email");
    }
}