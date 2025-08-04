using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAuthRepository
{
    Task<User?> FindUserByEmail(string email, CancellationToken cancellationToken);
    Task<User?> FindUserByPhoneNumber(string phoneNumber, CancellationToken cancellationToken);
    Task<Role?> FindRoleByName(string roleName, CancellationToken cancellationToken);
    Task<UserLog> AddUserLog(UserLog userLog, CancellationToken cancellationToken);
    Task<User> AddUser(User user, CancellationToken cancellationToken);
    Task<User?> FindUserAndUserLogByVerificationCode(string verificationCode, CancellationToken cancellationToken);
    Task<User?> FindUserAndUserLogAndRoleByEmail(string email, CancellationToken cancellationToken = default);
    Task<User?> FindUserAndUserLogByRefreshToken(string refreshToken, CancellationToken cancellationToken = default);
    Task<UserAssociatedDataDto?> GetUserAssociatedDataByIdUser(Guid idUser, CancellationToken cancellationToken);
    Task<UserLog?> FindUserLogByResetEmailToken(Guid resetEmailToken, CancellationToken cancellationToken);
}