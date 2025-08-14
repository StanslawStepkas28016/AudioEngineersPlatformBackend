namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IUrlGeneratorUtil
{
    Task<string> GenerateResetVerificationUrl(Guid token, string path);
}