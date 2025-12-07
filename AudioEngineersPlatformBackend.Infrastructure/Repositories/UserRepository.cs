using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AudioEngineersPlatformBackend.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AudioEngineersPlatformDbContext _context;

    public UserRepository(
        AudioEngineersPlatformDbContext context
    )
    {
        _context = context;
    }

    public async Task<bool> DoesUserExistByIdUserAsync(
        Guid idUser,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .Users
            .AnyAsync(u => u.IdUser == idUser, cancellationToken);
    }

    public async Task<bool> AreUsersInTheSameRoleAsync(
        Guid idFirstUser,
        Guid idSecondUser,
        CancellationToken cancellationToken
    )
    {
        Guid idRoleFirstUser = await _context
            .Users
            .Where(u => u.IdUser == idFirstUser)
            .Select(u => u.IdRole)
            .FirstOrDefaultAsync(cancellationToken);

        Guid idRoleSecondUser = await _context
            .Users
            .Where(u => u.IdUser == idSecondUser)
            .Select(u => u.IdRole)
            .FirstOrDefaultAsync(cancellationToken);

        return idRoleFirstUser == idRoleSecondUser;
    }

    public async Task<Tuple<string, string>> FindUserInfoByIdUserAsync(
        Guid idUserSenderValidated,
        CancellationToken cancellationToken
    )
    {
        return (await _context
            .Users
            .Where(u => u.IdUser == idUserSenderValidated)
            .Select
            (u => Tuple.Create
                (
                    u.FirstName,
                    u.LastName
                )
            )
            .FirstOrDefaultAsync(cancellationToken))!;
    }
}