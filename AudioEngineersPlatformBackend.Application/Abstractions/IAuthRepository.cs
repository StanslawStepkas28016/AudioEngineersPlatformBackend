using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAuthRepository
{
    Task<User?> FindUserByEmailAsNoTrackingAsync(
        string email,
        CancellationToken cancellationToken = default
    );

    Task<bool> IsPhoneNumberAlreadyTakenAsync(
        string phoneNumber,
        CancellationToken cancellationToken
    );

    Task<bool> IsEmailAlreadyTakenAsync(
        string email,
        CancellationToken cancellationToken
    );

    Task<Role?> FindRoleByNameAsNoTrackingAsync(
        string roleName,
        CancellationToken cancellationToken
    );

    Task<UserAuthLog> AddUserLogAsync(
        UserAuthLog userAuthLog,
        CancellationToken cancellationToken
    );

    Task AddTokenAsync(
        Token token,
        CancellationToken cancellationToken
    );

    Task AddUserAsync(
        User user,
        CancellationToken cancellationToken
    );

    Task<User?> FindUserAndUserLogAndTokenByTokenAsync(
        string tokenValue,
        CancellationToken cancellationToken
    );

    Task<User?> FindUserAndUserLogAndRoleByEmailAsync(
        string email,
        CancellationToken cancellationToken = default
    );

    Task<User?> FindUserAndUserLogByIdUserAsync(
        Guid idUser,
        CancellationToken cancellationToken
    );

    Task<CheckAuthDto?> GetCheckAuthDataAsync(
        Guid idUser,
        CancellationToken cancellationToken
    );

    Task DeleteTokenByValueAsync(
        string value,
        CancellationToken cancellationToken
    );

    Task DeleteAllTokensWithSpecificNameByIdUserAsync(
        Guid idUser,
        string tokenName,
        CancellationToken cancellationToken
    );
}