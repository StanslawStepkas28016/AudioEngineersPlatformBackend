using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Infrastructure.Context;
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

    public async Task<User?> FindUserByEmailAsNoTrackingAsync(string email,
        CancellationToken cancellationToken = default)
    {
        return await _context
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> IsPhoneNumberAlreadyTaken(string phoneNumber, CancellationToken cancellationToken)
    {
        return await _context
            .Users
            .AnyAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);
    }


    public async Task<Role?> FindRoleByNameAsNoTrackingAsync(string roleName, CancellationToken cancellationToken)
    {
        return await _context
            .Roles
            .AsNoTracking()
            .FirstOrDefaultAsync
            (
                r => r.RoleName == roleName,
                cancellationToken
            );
    }

    public async Task<UserAuthLog> AddUserLogAsync(UserAuthLog userAuthLog, CancellationToken cancellationToken)
    {
        EntityEntry<UserAuthLog> res = await _context
            .UserAuthLogs
            .AddAsync(userAuthLog, cancellationToken);

        return
            res.Entity;
    }

    public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken)
    {
        EntityEntry<User> res = await _context
            .Users
            .AddAsync(user, cancellationToken);

        return
            res.Entity;
    }

    public async Task<User?> FindUserAndUserLogByVerificationCodeAsync(string verificationCode,
        CancellationToken cancellationToken)
    {
        return await _context
            .Users
            .Include(u => u.UserAuthLog)
            .FirstOrDefaultAsync
            (
                u => u.UserAuthLog.VerificationCode == verificationCode,
                cancellationToken
            );
    }

    public async Task<User?> FindUserAndUserLogAndRoleByEmailAsync(string email,
        CancellationToken cancellationToken = default)
    {
        return await _context
            .Users
            .Include(u => u.Role)
            .Include(u => u.UserAuthLog)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> FindUserAndUserLogByRefreshTokenAsync(string refreshToken,
        CancellationToken cancellationToken = default)
    {
        return await _context
            .Users
            .Include(u => u.UserAuthLog)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserAuthLog.RefreshToken == refreshToken, cancellationToken);
    }

    public async Task<UserAssociatedDataDto?> GetUserAssociatedDataByIdUserAsync(Guid idUser,
        CancellationToken cancellationToken)
    {
        return await _context
            .Users
            .Where(u => u.IdUser == idUser)
            .Select
            (u => new UserAssociatedDataDto
                {
                    IdUser = u.IdUser!,
                    Email = u.Email,
                    FirstName = u.FirstName!,
                    LastName = u.LastName!,
                    PhoneNumber = u.PhoneNumber!,
                    IdRole = u.IdRole,
                    RoleName = u.Role.RoleName!
                }
            )
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<UserAuthLog?> FindUserLogByResetEmailTokenAsync(Guid resetEmailToken,
        CancellationToken cancellationToken)
    {
        return await _context
            .UserAuthLogs
            .FirstOrDefaultAsync(ul => ul.ResetEmailToken == resetEmailToken, cancellationToken);
    }

    public async Task<UserAuthLog?> FindUserLogByResetPasswordTokenAsync(Guid resetPasswordToken,
        CancellationToken cancellationToken)
    {
        return await _context
            .UserAuthLogs
            .FirstOrDefaultAsync(ul => ul.ResetPasswordToken == resetPasswordToken, cancellationToken);
    }

    public async Task<User?> FindUserAndUserLogByForgotPasswordToken(Guid forgotPasswordToken,
        CancellationToken cancellationToken)
    {
        return await _context
            .Users
            .Include(u => u.UserAuthLog)
            .Where(u => u.UserAuthLog.ForgotPasswordToken == forgotPasswordToken)
            .FirstOrDefaultAsync(cancellationToken);
    }
}