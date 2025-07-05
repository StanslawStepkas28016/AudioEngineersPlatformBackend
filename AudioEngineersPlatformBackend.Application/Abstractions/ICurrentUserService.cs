namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface ICurrentUserService
{
    Guid IdUser { get; }
    bool IsAdministrator { get; }
}