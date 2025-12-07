namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.ChangeAdvertData;

public class ChangeAdvertDataCommandResult
{
    public required Guid IdUser { get; set; }
    public required Guid IdAdvert { get; set; }
}