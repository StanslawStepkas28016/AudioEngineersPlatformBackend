namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IUrlGeneratorUtil
{
    string GenerateResetEmailUrl(Guid emailResetToken);
}