using AudioEngineersPlatformBackend.Contracts.User.ChangeData;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IUserService
{
    public Task<Guid> ChangeData(Guid idUser, ChangeDataRequest changeDataRequest, CancellationToken cancellationToken);
}