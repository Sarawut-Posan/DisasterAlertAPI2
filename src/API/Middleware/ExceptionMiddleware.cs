using API.Models;
using Application.Exceptions;
using FluentValidation;

namespace API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, ex.Message);

        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Timestamp = DateTime.UtcNow
        };

        switch (ex)
        {
            case ApiException apiException:
                context.Response.StatusCode = apiException.StatusCode;
                response.StatusCode = apiException.StatusCode;
                response.Message = apiException.Message;
                break;

            case Exception:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Message = _env.IsDevelopment() 
                    ? ex.Message 
                    : "An unexpected error occurred.";
                response.Details = _env.IsDevelopment() 
                    ? ex.StackTrace 
                    : null;
                break;
        }

        await context.Response.WriteAsJsonAsync(response);
    }
}