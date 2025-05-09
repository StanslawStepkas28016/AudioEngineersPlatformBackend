using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AudioEngineersPlatformBackend.Infrastructure.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly EngineersPlatformDbContext _context;

    public AuthenticationRepository(EngineersPlatformDbContext context)
    {
        _context = context;
    }

    public async Task<User?> FindUserByEmail(string email, CancellationToken cancellationToken = default)
    {
        return await _context
            .Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> FindUserByPhoneNumber(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await _context
            .Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<Role?> FindRoleByName(string roleName, CancellationToken cancellationToken)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName,
            cancellationToken);
    }

    public async Task<UserLog> AddUserLog(UserLog userLog, CancellationToken cancellationToken)
    {
        var res = await _context
            .UserLogs
            .AddAsync(userLog, cancellationToken);

        return
            res.Entity;
    }

    public async Task<User> AddUser(User user, CancellationToken cancellationToken)
    {
        var res = await _context
            .Users
            .AddAsync(user, cancellationToken);

        return
            res.Entity;
    }
}