using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IUserRepository
{
    Task<Guid> FindUserByIdUser(Guid idUser, CancellationToken cancellationToken);
}