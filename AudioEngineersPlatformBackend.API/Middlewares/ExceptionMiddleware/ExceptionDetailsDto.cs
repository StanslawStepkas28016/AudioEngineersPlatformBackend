using System.Text.Json;

namespace API.Middlewares.ExceptionMiddleware;

public class ExceptionDetailsDto
{
    public int StatusCode { get; set; }
    public string FromClass { get; set; }
    public string FromMethod { get; set; }
    public string FromLine { get; set; }
    public string ExceptionMessage { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}