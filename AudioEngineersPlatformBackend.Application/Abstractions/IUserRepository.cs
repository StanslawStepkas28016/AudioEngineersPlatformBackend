using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IUserRepository
{
    Task<bool> DoesUserExistByIdUser(Guid idUser, CancellationToken cancellationToken);

    Task<bool> IsEmailAlreadyTaken(string email, CancellationToken cancellationToken);

    Task<User?> FindUserByIdUser(Guid idUser, CancellationToken cancellationToken);
    
    Task<bool> IsPhoneNumberAlreadyTaken(string phoneNumber, CancellationToken cancellationToken);

    Task<UserLog?> FindUserLogByIdUser(Guid idUser, CancellationToken cancellationToken);
}