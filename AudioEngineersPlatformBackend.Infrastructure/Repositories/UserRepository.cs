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

    public async Task<UserLog?> FindUserLogByIdUserAsync(Guid idUser, CancellationToken cancellationToken)
    {
        return await _context
            .Users
            .Where(u => u.IdUser == idUser)
            .Select(u => u.UserLog)
            .FirstOrDefaultAsync(cancellationToken);
    }
}