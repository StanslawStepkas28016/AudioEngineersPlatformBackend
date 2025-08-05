using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAuthRepository
{
    Task<User?> FindUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> FindUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<Role?> FindRoleByNameAsync(string roleName, CancellationToken cancellationToken);
    Task<UserLog> AddUserLogAsync(UserLog userLog, CancellationToken cancellationToken);
    Task<User> AddUserAsync(User user, CancellationToken cancellationToken);
    Task<User?> FindUserAndUserLogByVerificationCodeAsync(string verificationCode, CancellationToken cancellationToken);
    Task<User?> FindUserAndUserLogAndRoleByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> FindUserAndUserLogByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<UserAssociatedDataDto?> GetUserAssociatedDataByIdUserAsync(Guid idUser, CancellationToken cancellationToken);
    Task<UserLog?> FindUserLogByResetEmailTokenAsync(Guid resetEmailToken, CancellationToken cancellationToken);
}