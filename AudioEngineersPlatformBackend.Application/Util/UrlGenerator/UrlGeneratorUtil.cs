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

    public Task<string> GenerateResetVerificationUrl(Guid token, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException($"{path} cannot be empty.");
        }
        
        return Task.FromResult($"{FrontendSettings.Url}/{token}/{path}");
    }
}