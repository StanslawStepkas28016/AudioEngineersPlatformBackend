using AudioEngineersPlatformBackend.Contracts.Authentication;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAuthenticationRepository
{
    Task<User?> FindUserByEmail(string email, CancellationToken cancellationToken);
    Task<User?> FindUserByPhoneNumber(string phoneNumber, CancellationToken cancellationToken);
    Task<Role?> FindRoleByName(string roleName, CancellationToken cancellationToken);
    Task<UserLog> AddUserLog(UserLog userLog, CancellationToken cancellationToken);
    Task<User> AddUser(User user, CancellationToken cancellationToken);
    Task<User?> FindUserAndUserLogByIdAndVerificationCode(Guid idUser, string verificationCode,
        CancellationToken cancellationToken);
    Task<User?> FindUserAndUserLogAndRoleByEmail(string email, CancellationToken cancellationToken = default);
}