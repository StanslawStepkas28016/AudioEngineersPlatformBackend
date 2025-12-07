namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IUrlGeneratorUtil
{
    public Task<string> GenerateResetVerificationUrl(
        string tokenValue,
        string path
    );
}