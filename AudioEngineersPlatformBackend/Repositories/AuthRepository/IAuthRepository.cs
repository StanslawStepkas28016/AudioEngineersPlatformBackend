using System.Threading;
using System.Threading.Tasks;
using AudioEngineersPlatformBackend.Dtos.Auth.Utilities;
using AudioEngineersPlatformBackend.Models;

namespace AudioEngineersPlatformBackend.Repositories.AuthRepository;

public interface IAuthRepository
{
    public Task<User?> FindUserByUsername(string username, CancellationToken cancellationToken);

    public Task<User?> FindUserByPhoneNumber(string phoneNumber, CancellationToken cancellationToken);

    public Task<User?> FindUserByEmail(string email, CancellationToken cancellationToken);

    public Task<Role?> FindRoleByName(string roleName, CancellationToken cancellationToken);

    public Task<UserAndUserLogDto?> StoreUserAndUserLogInDatabase(User user, UserLog userLog,
        CancellationToken cancellationToken);

    public Task<UserLog?> FindUserLogByVerificationCode(string verificationCode,
        CancellationToken cancellationToken);

    public Task<UserLog?> SetUserLogToVerifiedAndAdjustAssociatedData(int idUserLog,
        CancellationToken cancellationToken);

    public Task<UserLog?> FindUserLogByIdUserLog(int idUserLog,
        CancellationToken cancellationToken);

    public Task<UserLog?> SetUserLogLastLoginDateByIdUserLog(int idUserLog, CancellationToken cancellationToken);
}