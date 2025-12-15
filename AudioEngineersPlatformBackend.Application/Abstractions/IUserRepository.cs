namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IUserRepository
{
    Task<bool> DoesUserExistByIdUserAsync(
        Guid idUser,
        CancellationToken cancellationToken
    );

    Task<bool> AreUsersInTheSameRoleAsync(
        Guid idFirstUser,
        Guid idSecondUser,
        CancellationToken cancellationToken
    );

    Task<Tuple<string, string>> FindUserInfoByIdUserAsync(
        Guid idUserSenderValidated,
        CancellationToken cancellationToken
    );
}