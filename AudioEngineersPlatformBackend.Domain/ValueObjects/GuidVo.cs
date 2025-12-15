namespace AudioEngineersPlatformBackend.Domain.ValueObjects;

public readonly struct GuidVo
{
    private readonly Guid _guid;

    public Guid Guid
    {
        get => _guid;
        init
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(Guid)} cannot be empty!");
            }

            _guid = value;
        }
    }

    public GuidVo(Guid guid)
    {
        Guid = guid;
    }
}