using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Config.Settings;
using Microsoft.Extensions.Options;

namespace AudioEngineersPlatformBackend.Application.Util.UrlGeneratorUtil;

public class UrlGeneratorUtil : IUrlGeneratorUtil
{
    private readonly FrontendSettings _frontendSettings;

    public UrlGeneratorUtil(
        IOptions<FrontendSettings> frontendSettings
    )
    {
        _frontendSettings = frontendSettings.Value;
    }

    public Task<string> GenerateResetVerificationUrl(
        string tokenValue,
        string path
    )
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException($"{path} cannot be empty.");
        }

        return Task.FromResult($"{_frontendSettings.Url}/{tokenValue}/{path}");
    }
}