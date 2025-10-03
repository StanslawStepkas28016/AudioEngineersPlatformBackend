using System.Text.Json;

namespace API.Dtos;

public class ExceptionDetailsDto
{
    public int StatusCode { get; set; }
    public string ExceptionMessage { get; set; } 

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}