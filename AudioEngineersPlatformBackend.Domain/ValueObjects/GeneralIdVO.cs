namespace AudioEngineersPlatformBackend.Domain.ValueObjects;

public class GeneralIdVo
{
    private readonly Guid _id;

    public GeneralIdVo(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Provided Id cannot be empty", nameof(id));
        }

        _id = id;
    }

    public Guid GetValidId()
    {
        return _id;
    }
}