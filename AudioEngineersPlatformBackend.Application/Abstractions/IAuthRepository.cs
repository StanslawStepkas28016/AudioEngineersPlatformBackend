using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAuthRepository
{
    Task<User?> FindUserByEmailAsNoTrackingAsync(
        string email,
        CancellationToken cancellationToken
    );

    Task<bool> IsPhoneNumberAlreadyTaken(
        string phoneNumber,
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

    Task<User> AddUserAsync(
        User user,
        CancellationToken cancellationToken
    );

    Task<User?> FindUserAndUserLogByVerificationCodeAsync(
        string verificationCode,
        CancellationToken cancellationToken
    );

    Task<User?> FindUserAndUserLogAndRoleByEmailAsync(
        string email,
        CancellationToken cancellationToken = default
    );

    Task<User?> FindUserAndUserLogByRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default
    );

    Task<UserAssociatedDataDto?> GetUserAssociatedDataByIdUserAsync(
        Guid idUser,
        CancellationToken cancellationToken
    );

    Task<UserAuthLog?> FindUserLogByResetEmailTokenAsync(
        Guid resetEmailToken,
        CancellationToken cancellationToken
    );

    Task<UserAuthLog?> FindUserLogByResetPasswordTokenAsync(
        Guid resetPasswordToken,
        CancellationToken cancellationToken
    );

    Task<User?> FindUserAndUserLogByForgotPasswordToken(
        Guid forgotPasswordToken,
        CancellationToken cancellationToken
    );
}