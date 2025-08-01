namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IUrlGeneratorUtil
{
    string ConstructResetEmailUrl(Guid emailResetToken);
}