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
}