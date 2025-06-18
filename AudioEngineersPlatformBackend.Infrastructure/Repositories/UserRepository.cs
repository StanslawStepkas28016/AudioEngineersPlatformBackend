using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AudioEngineersPlatformBackend.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly EngineersPlatformDbContext _context;

    public UserRepository(EngineersPlatformDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DoesUserExistByIdUserAsync(Guid idUser, CancellationToken cancellationToken)
    {
        return await _context
            .Users
            .Where(u => u.IdUser == idUser)
            .Select(u => u.IdUser)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> IsEmailAlreadyTakenAsync(string email, CancellationToken cancellationToken)
    {
        return
            await _context
                .Users
                .Where(u => u.Email == email)
                .AnyAsync(cancellationToken);
    }

    public async Task<User?> FindUserByIdUserAsync(Guid idUser, CancellationToken cancellationToken)
    {
        return await _context
            .Users
            .Where(u => u.IdUser == idUser)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsPhoneNumberAlreadyTakenAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return await _context
            .Users
            .Where(u => u.PhoneNumber == phoneNumber)
            .AnyAsync(cancellationToken);
    }

    public async Task<UserAuthLog?> FindUserLogByIdUserAsync(Guid idUser, CancellationToken cancellationToken)
    {
        return await _context
            .Users
            .Where(u => u.IdUser == idUser)
            .Select(u => u.UserAuthLog)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> AreInTheSameRole(Guid idFirstUser, Guid idSecondUser, CancellationToken cancellationToken)
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

    public async Task<Tuple<string, string>> FindUserInfoByIdUserAsync(Guid idUserSenderValidated, CancellationToken cancellationToken)
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
            ).FirstOrDefaultAsync(cancellationToken))!;
    }
}