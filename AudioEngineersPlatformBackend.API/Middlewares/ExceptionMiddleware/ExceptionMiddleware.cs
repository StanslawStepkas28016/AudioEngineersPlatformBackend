using System.Net;

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
        catch (ArgumentNullException ex)
        {
            await HandleArgumentNullExceptionAsync(context, ex);
        }
        catch (ArgumentException ex)
        {
            await HandleArgumentExceptionAsync(context, ex);
        }

        catch (UnauthorizedAccessException ex)
        {
            await HandleUnauthorizedAccessExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleGeneralExceptionAsync(context, ex);
        }
    }

    private Task HandleArgumentExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return context.Response.WriteAsync(new ErrorDetailsDto
        {
            StatusCode = context.Response.StatusCode,
            Message = "Error caused by incorrect arguments.",
            ExceptionMessage = exception.Message
        }.ToString());
    }

    private Task HandleArgumentNullExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;

        return context.Response.WriteAsync(new ErrorDetailsDto
        {
            StatusCode = context.Response.StatusCode,
            Message = "Error caused by incorrect (null) arguments.",
            ExceptionMessage = exception.Message
        }.ToString());
    }


    private Task HandleUnauthorizedAccessExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

        return context.Response.WriteAsync(new ErrorDetailsDto
        {
            StatusCode = context.Response.StatusCode,
            Message = "Unauthorized access.",
            ExceptionMessage = exception.Message
        }.ToString());
    }

    private Task HandleGeneralExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(new ErrorDetailsDto
        {
            StatusCode = context.Response.StatusCode,
            Message = "Internal Server Error from the custom middleware.",
            ExceptionMessage = exception.Message
        }.ToString());
    }
}