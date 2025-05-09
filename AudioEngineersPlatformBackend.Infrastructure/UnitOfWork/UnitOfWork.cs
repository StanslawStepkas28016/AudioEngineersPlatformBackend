using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Infrastructure.Context;

namespace AudioEngineersPlatformBackend.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly EngineersPlatformDbContext _context;

    public UnitOfWork(EngineersPlatformDbContext context)
    {
        _context = context;
    }

    public async Task CompleteAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}