namespace API.Contracts.Advert.Commands.CreateAdvert;

public class CreateAdvertResponse
{
    public required Guid IdUser { get; set; }
    public required Guid IdAdvert { get; set; }
}