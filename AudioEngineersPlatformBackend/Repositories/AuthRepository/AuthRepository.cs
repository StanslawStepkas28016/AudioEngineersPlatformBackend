using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AudioEngineersPlatformBackend.Dtos.Auth.Utilities;
using AudioEngineersPlatformBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AudioEngineersPlatformBackend.Repositories.AuthRepository;

public class AuthRepository : IAuthRepository
{
    private readonly MasterContext _context;

    public AuthRepository(MasterContext context)
    {
        _context = context;
    }

    public async Task<User?> FindUserByUsername(string username, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username,
            cancellationToken: cancellationToken);
    }

    public async Task<User?> FindUserByPhoneNumber(string phoneNumber, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber,
            cancellationToken: cancellationToken);
    }

    public async Task<User?> FindUserByEmail(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken: cancellationToken);
    }

    public async Task<Role?> FindRoleByName(string roleName, CancellationToken cancellationToken)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName,
            cancellationToken: cancellationToken);
    }

    public async Task<UserLog?> StoreUserLog(UserLog userLog, CancellationToken cancellationToken)
    {
        // Save user data to UserLog table
        var userLogRes = await _context.UserLogs.AddAsync(userLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return userLogRes.Entity;
    }

    public async Task<User?> StoreUser(User user,
        CancellationToken cancellationToken)
    {
        // Save user data to User table 
        var userRes = await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return userRes.Entity;
    }

    public async Task<UserLog?> FindUserLogByVerificationCode(string verificationCode,
        CancellationToken cancellationToken)
    {
        return await _context
            .UserLogs
            .Where(ul => ul.VerificationCode == verificationCode)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<UserLog?> SetUserLogToVerifiedAndAdjustAssociatedData(int idUserLog,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var userLogRes = await _context
                .UserLogs
                .FirstOrDefaultAsync(ul => ul.IdUserLog == idUserLog, cancellationToken);

            // Adjust associated data
            userLogRes!.IsVerified = true;
            userLogRes.VerificationCode = null;
            userLogRes.VerificationCodeExpiration = null;
            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return userLogRes;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<UserLog?> FindUserLogByIdUserLog(int idUserLog,
        CancellationToken cancellationToken)
    {
        return await _context
            .UserLogs
            .FirstOrDefaultAsync(ul => ul.IdUserLog == idUserLog, cancellationToken);
    }

    public async Task<UserLog?> SetUserLogLastLoginDateByIdUserLog(int idUserLog, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var userLogRes = await _context
                .UserLogs
                .FirstOrDefaultAsync(ul => ul.IdUserLog == idUserLog, cancellationToken);

            userLogRes!.DateLastLogin = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return userLogRes;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}