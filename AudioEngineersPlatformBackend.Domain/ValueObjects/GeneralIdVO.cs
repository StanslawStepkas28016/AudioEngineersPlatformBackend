namespace AudioEngineersPlatformBackend.Domain.ValueObjects;

public readonly struct GeneralIdVo
{
    private readonly Guid _id;

    public Guid Id
    {
        get { return _id; }
        init
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(Id)} from {GetType()} cannot be empty.");
            }

            _id = value;
        }
    }

    public GeneralIdVo(Guid id)
    {
        Id = id;
    }

    public Guid GetValidId()
    {
        return _id;
    }
}