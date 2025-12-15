namespace API.Contracts.Advert.Commands.ChangeAdvertData;

public class ChangeAdvertDataResponse
{
    public required Guid IdUser { get; set; }
    public required Guid IdAdvert { get; set; }
}