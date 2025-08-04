namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IUrlGeneratorUtil
{
    Task<string> GenerateResetEmailUrl(Guid emailResetToken);
}