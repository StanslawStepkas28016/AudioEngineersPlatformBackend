using System.Diagnostics;
using System.Net;
using Serilog;

namespace API.Middlewares.ExceptionMiddleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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

        StackTrace trace = new StackTrace(exception, true);

        ExceptionDetailsDto exceptionDetailsDto = new ExceptionDetailsDto
        {
            StatusCode = context.Response.StatusCode,
            FromClass = trace.GetFrame(0)!.GetMethod()!.ReflectedType!.FullName!,
            FromMethod = trace.GetFrame(0)!.GetMethod()!.DeclaringType!.Name,
            FromLine = trace.GetFrame(0)!.GetFileLineNumber().ToString(),
            ExceptionMessage = exception.Message
        };

        _logger.LogError
        (
            "An error has occurred in the {FromClass} class in the {FromMethod} method with the following message: {ExceptionMessage}",
            exceptionDetailsDto.FromClass,
            exceptionDetailsDto.FromMethod,
            exceptionDetailsDto.ExceptionMessage
        );

        return context.Response.WriteAsync(exceptionDetailsDto.ToString());
    }
}