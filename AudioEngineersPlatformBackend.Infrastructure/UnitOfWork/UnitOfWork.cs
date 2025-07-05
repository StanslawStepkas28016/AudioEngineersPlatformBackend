using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AudioEngineersPlatformBackend.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    public UnitOfWork(EngineersPlatformDbContext context)
    {
        _context = context;
    }

    public async Task CompleteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}