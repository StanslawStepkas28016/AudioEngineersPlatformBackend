using System;
using System.Net;
using System.Threading.Tasks;
using AudioEngineersPlatformBackend.Dtos.Middlewares;
using AudioEngineersPlatformBackend.Exceptions;
using AudioEngineersPlatformBackend.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace AudioEngineersPlatformBackend.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public ExceptionMiddleware(RequestDelegate next, IStringLocalizer<ErrorMessages> localizer)
    {
        _next = next;
        _localizer = localizer;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (LocalizedGeneralException ex)
        {
            await HandleLocalizedGeneralException(context, ex);
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
    
    private Task HandleLocalizedGeneralException(HttpContext context, LocalizedGeneralException localizedGeneralException)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var localizedMessage = _localizer[localizedGeneralException.ErrorKey, localizedGeneralException.FormatParameters];
        return context.Response.WriteAsync(new ErrorDetailsDto
        {
            StatusCode = context.Response.StatusCode,
            Message = "Internal Server Error from custom middleware (localized).",
            ExceptionMessage = localizedMessage
        }.ToString());
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