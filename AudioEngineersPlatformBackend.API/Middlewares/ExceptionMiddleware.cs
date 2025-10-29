using System.Net;
using API.Dtos;
using AudioEngineersPlatformBackend.Domain.Exceptions;

namespace API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context
    )
    {
        try
        {
            await _next(context);
        }
        catch (BusinessRelatedException ex)
        {
            await HandleBusinessRelatedExceptionAsync(context, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleUnauthorizedAccessExceptionAsync(context, ex);
        }
        catch (ArgumentException ex)
        {
            await HandleArgumentExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleGeneralExceptionAsync(context, ex);
        }
    }

    /// <summary>
    ///     Used for passing business related data to the client,
    ///     all the exception messages passed are non-descriptive.
    ///     All throwing methods are supposed to log business related errors
    ///     for better debugging.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    private Task HandleBusinessRelatedExceptionAsync(
        HttpContext context,
        BusinessRelatedException exception
    )
    {
        // Log error.
        _logger.LogError
        (
            "Business related Error occurred with the following: {Message}",
            exception.Message
        );

        // Write the response details.
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        ExceptionDetailsDto exceptionDetailsDto = new ExceptionDetailsDto
        {
            StatusCode = context.Response.StatusCode,
            ExceptionMessage = exception.Message
        };

        return context.Response.WriteAsync(exceptionDetailsDto.ToString());
    }

    private Task HandleUnauthorizedAccessExceptionAsync(
        HttpContext context,
        UnauthorizedAccessException exception
    )
    {
        // Log error.
        _logger.LogError
        (
            "Authorization related Error occurred with the following: {Message}",
            exception.Message
        );

        // Write the response details.
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        ExceptionDetailsDto exceptionDetailsDto = new ExceptionDetailsDto
        {
            StatusCode = context.Response.StatusCode,
            ExceptionMessage = exception.Message
        };

        return context.Response.WriteAsync(exceptionDetailsDto.ToString());
    }

    private Task HandleArgumentExceptionAsync(
        HttpContext context,
        ArgumentException exception
    )
    {
        // Log error.
        _logger.LogError
        (
            "Argument related Error occurred with the following: {Message}",
            exception.Message
        );

        // Write the response details.
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        ExceptionDetailsDto exceptionDetailsDto = new ExceptionDetailsDto
        {
            StatusCode = context.Response.StatusCode,
            ExceptionMessage = exception.Message
        };

        return context.Response.WriteAsync(exceptionDetailsDto.ToString());
    }

    private Task HandleGeneralExceptionAsync(
        HttpContext context,
        Exception exception
    )
    {
        // Log error.
        _logger.LogError
        (
            "Internal Error occurred with the following: {ExceptionTrace}",
            exception
        );

        // Write the response details.
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        ExceptionDetailsDto exceptionDetailsDto = new ExceptionDetailsDto
        {
            StatusCode = context.Response.StatusCode,
            ExceptionMessage = "An error has occured."
        };

        return context.Response.WriteAsync(exceptionDetailsDto.ToString());
    }
}