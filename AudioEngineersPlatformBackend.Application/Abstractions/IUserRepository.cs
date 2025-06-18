using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IUserRepository
{
    Task<bool> DoesUserExistByIdUserAsync(Guid idUser, CancellationToken cancellationToken);

    Task<bool> IsEmailAlreadyTakenAsync(string email, CancellationToken cancellationToken);

    Task<User?> FindUserByIdUserAsync(Guid idUser, CancellationToken cancellationToken);
    
    Task<bool> IsPhoneNumberAlreadyTakenAsync(string phoneNumber, CancellationToken cancellationToken);

    Task<UserAuthLog?> FindUserLogByIdUserAsync(Guid idUser, CancellationToken cancellationToken);

    Task<bool> AreInTheSameRole(Guid idFirstUser, Guid idSecondUser, CancellationToken cancellationToken);
    Task<Tuple<string, string>> FindUserInfoByIdUserAsync(Guid idUserSenderValidated, CancellationToken cancellationToken);
}