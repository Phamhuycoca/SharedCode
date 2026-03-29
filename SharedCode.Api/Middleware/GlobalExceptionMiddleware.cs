using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SharedCode.Application.Responses;

namespace SharedCode.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var traceId = context.TraceIdentifier;

        if (exception is AppException appEx)
        {
            _logger.LogWarning(
                "Business exception | TraceId: {TraceId} | Message: {Message}",
                traceId,
                appEx.Message
            );
        }
        else
        {
            _logger.LogError(exception,
                "Unhandled exception | TraceId: {TraceId} | Method: {Method} | Path: {Path}",
                traceId,
                context.Request.Method,
                context.Request.Path
            );
        }

        var (statusCode, message, detail) = MapException(exception);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ApiErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = message,
            //Detail = IsDevelopment() ? detail : null,
            TraceId = traceId
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response)
        );
    }

    // 👉 MAP EXCEPTION ĐẶT Ở ĐÂY
    private static (HttpStatusCode, string, string?) MapException(Exception ex)
    {
        return ex switch
        {
            AppException appEx => (
                appEx.StatusCode,
                appEx.Message,
                appEx.Detail
            ),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "Unauthorized",
                ex.Message
            ),

            ArgumentException => (
                HttpStatusCode.BadRequest,
                ex.Message,
                ex.StackTrace
            ),

            _ => (
                HttpStatusCode.InternalServerError,
                "Internal server error",
                ex.Message
            )
        };
    }

    private static bool IsDevelopment()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
               == Environments.Development;
    }
}
