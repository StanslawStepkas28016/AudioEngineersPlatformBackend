namespace AudioEngineersPlatformBackend.Domain.Exceptions;

public class BusinessRelatedException : Exception
{
    public BusinessRelatedException(
        string? message
    ) : base(message)
    {
    }
}