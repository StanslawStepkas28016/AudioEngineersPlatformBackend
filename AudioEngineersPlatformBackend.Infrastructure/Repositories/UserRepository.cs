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

    public async Task<Guid> FindUserByIdUser(Guid idUser, CancellationToken cancellationToken)
    {
        return await _context
            .Users
            .Where(u => u.IdUser == idUser)
            .Select(u => u.IdUser)
            .FirstOrDefaultAsync(cancellationToken);
    }
}