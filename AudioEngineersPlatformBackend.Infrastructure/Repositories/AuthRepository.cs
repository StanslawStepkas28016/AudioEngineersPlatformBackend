using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AudioEngineersPlatformBackend.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly EngineersPlatformDbContext _context;

    public AuthRepository(EngineersPlatformDbContext context)
    {
        _context = context;
    }

    public async Task<User?> FindUserByEmail(string email, CancellationToken cancellationToken = default)
    {
        return await _context
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> FindUserByPhoneNumber(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await _context
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<Role?> FindRoleByName(string roleName, CancellationToken cancellationToken)
    {
        return await _context
            .Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RoleName == roleName,
            cancellationToken);
    }

    public async Task<UserLog> AddUserLog(UserLog userLog, CancellationToken cancellationToken)
    {
        EntityEntry<UserLog> res = await _context
            .UserLogs
            .AddAsync(userLog, cancellationToken);

        return
            res.Entity;
    }

    public async Task<User> AddUser(User user, CancellationToken cancellationToken)
    {
        EntityEntry<User> res = await _context
            .Users
            .AddAsync(user, cancellationToken);

        return
            res.Entity;
    }

    public async Task<User?> FindUserAndUserLogByVerificationCode(string verificationCode,
        CancellationToken cancellationToken)
    {
        return await _context
            .Users
            .Include(u => u.UserLog)
            .FirstOrDefaultAsync(u => u.UserLog.VerificationCode == verificationCode,
                cancellationToken);
    }

    public async Task<User?> FindUserAndUserLogAndRoleByEmail(string email,
        CancellationToken cancellationToken = default)
    {
        return await _context
            .Users
            .Include(u => u.Role)
            .Include(u => u.UserLog)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> FindUserAndUserLogByRefreshToken(string refreshToken,
        CancellationToken cancellationToken = default)
    {
        return await _context
            .Users
            .Include(u => u.UserLog)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserLog.RefreshToken == refreshToken, cancellationToken);
    }

    public async Task<UserAssociatedDataDto?> GetUserAssociatedDataByIdUser(Guid idUser,
        CancellationToken cancellationToken)
    {
        return await _context
            .Users
            .Where(u => u.IdUser == idUser)
            .Select(u => new UserAssociatedDataDto
            {
                IdUser = u.IdUser!,
                Email = u.Email,
                FirstName = u.FirstName!,
                LastName = u.LastName!,
                PhoneNumber = u.PhoneNumber!,
                IdRole = u.IdRole,
                RoleName = u.Role.RoleName!
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}