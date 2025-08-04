using System.Diagnostics;
using System.Net;
using API.Extensions;

namespace API.Middlewares.ExceptionMiddleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleGeneralExceptionAsync(context, ex);
        }
    }

    private Task HandleGeneralExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var trace = new StackTrace(exception, true);

        var exceptionDetailsDto = new ExceptionDetailsDto
        {
            StatusCode = context.Response.StatusCode,
            FromClass = trace.GetFrame(0)!.GetMethod()!.ReflectedType!.FullName!,
            FromMethod = trace.GetFrame(0)!.GetMethod()!.DeclaringType!.Name,
            FromLine = trace.GetFrame(0)!.GetFileLineNumber().ToString(),
            ExceptionMessage = exception.Message
        };

        Console.WriteLine();
        Console.WriteLine(exceptionDetailsDto.ToStringPretty());
        Console.WriteLine();

        return context.Response.WriteAsync(exceptionDetailsDto.ToString());
    }
}