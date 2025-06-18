namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface ICurrentUserUtil
{
    Guid IdUser { get; }
    bool IsAdministrator { get; }
}