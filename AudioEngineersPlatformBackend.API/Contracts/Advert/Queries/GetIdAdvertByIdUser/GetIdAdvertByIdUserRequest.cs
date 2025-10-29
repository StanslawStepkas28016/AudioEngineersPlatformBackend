using System.Text.Json.Serialization;

namespace API.Contracts.Advert.Queries.GetIdAdvertByIdUser;

public class GetIdAdvertByIdUserRequest
{
    public required Guid IdUser { get; set; }
}