using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AudioEngineersPlatformBackend.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AudioEngineersPlatformDbContext _context;

    public AuthRepository(
        AudioEngineersPlatformDbContext context
    )
    {
        _context = context;
    }

    public async Task<User?> FindUserByEmailAsNoTrackingAsync(
        string email,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> IsPhoneNumberAlreadyTakenAsync(
        string phoneNumber,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .Users
            .AnyAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<bool> IsEmailAlreadyTakenAsync(
        string email,
        CancellationToken cancellationToken
    )
    {
        return
            await _context
                .Users
                .Where(u => u.Email == email)
                .AnyAsync(cancellationToken);
    }

    public async Task<Role?> FindRoleByNameAsNoTrackingAsync(
        string roleName,
        CancellationToken cancellationToken
    )
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

    public async Task<UserAuthLog> AddUserLogAsync(
        UserAuthLog userAuthLog,
        CancellationToken cancellationToken
    )
    {
        EntityEntry<UserAuthLog> res = await _context
            .UserAuthLogs
            .AddAsync(userAuthLog, cancellationToken);

        return
            res.Entity;
    }

    public async Task AddTokenAsync(
        Token token,
        CancellationToken cancellationToken
    )
    {
        await _context
            .Tokens
            .AddAsync(token, cancellationToken);
    }

    public async Task AddUserAsync(
        User user,
        CancellationToken cancellationToken
    )
    {
        await _context
            .Users
            .AddAsync(user, cancellationToken);
    }

    public async Task<User?> FindUserAndUserLogAndTokenByTokenAsync(
        string tokenValue,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .Users
            .Include(u => u.Role)
            .Include(u => u.UserAuthLog)
            .Include
            (u => u.Tokens
                .Where(t => t.Value == tokenValue)
            )
            .FirstOrDefaultAsync
            (
                u => u.Tokens.Any(t => t.Value == tokenValue),
                cancellationToken
            );
    }

    public async Task<User?> FindUserAndUserLogAndRoleByEmailAsync(
        string email,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Users
            .Include(u => u.Role)
            .Include(u => u.UserAuthLog)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> FindUserAndUserLogByIdUserAsync(
        Guid idUser,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .Users
            .Include(u => u.Role)
            .Include(u => u.UserAuthLog)
            .FirstOrDefaultAsync(u => u.IdUser == idUser, cancellationToken);
    }

    public async Task<CheckAuthDto?> GetCheckAuthDataAsync(
        Guid idUser,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .Users
            .Where(u => u.IdUser == idUser)
            .Select
            (u => new CheckAuthDto
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

    public async Task DeleteTokenByValueAsync(
        string value,
        CancellationToken cancellationToken
    )
    {
        await _context
            .Tokens
            .Where(t => t.Value == value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task DeleteAllTokensWithSpecificNameByIdUserAsync(
        Guid idUser,
        string tokenName,
        CancellationToken cancellationToken
    )
    {
        await _context
            .Tokens
            .Where(t => t.IdUser == idUser && t.Name == tokenName)
            .ExecuteDeleteAsync(cancellationToken);
    }
}