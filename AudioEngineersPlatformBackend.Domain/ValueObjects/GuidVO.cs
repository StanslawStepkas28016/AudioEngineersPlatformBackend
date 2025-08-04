namespace AudioEngineersPlatformBackend.Domain.ValueObjects;

public readonly struct GuidVO
{
    private readonly Guid _guid;

    public GuidVO(Guid guid)
    {
        if (guid == Guid.Empty)
        {
            throw new ArgumentException($"{nameof(guid)} cannot be empty!");
        }

        _guid = guid;
    }

    public Guid GetValidGuid()
    {
        return _guid;
    }
}