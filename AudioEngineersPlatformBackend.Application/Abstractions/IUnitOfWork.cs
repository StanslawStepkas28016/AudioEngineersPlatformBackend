namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IUnitOfWork
{
    public Task CompleteAsync(CancellationToken cancellationToken);
}