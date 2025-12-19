using System.Net;
using System.Text.Json;
using FluentValidation;

namespace TeknoRoma.API.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;

        var errorResponse = new ErrorResponse
        {
            Success = false
        };

        switch (exception)
        {
            case ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = "Validation error occurred";
                errorResponse.Errors = validationException.Errors
                    .Select(e => new ErrorDetail
                    {
                        Field = e.PropertyName,
                        Message = e.ErrorMessage
                    }).ToList();
                break;

            case KeyNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Message = exception.Message;
                break;

            case UnauthorizedAccessException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.Message = "Unauthorized access";
                break;

            case ArgumentException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = exception.Message;
                break;

            case InvalidOperationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = exception.Message;
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "Internal server error occurred";
                errorResponse.Errors = new List<ErrorDetail>
                {
                    new ErrorDetail
                    {
                        Field = "Exception",
                        Message = exception.Message
                    }
                };
                break;
        }

        var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(result);
    }
}

public class ErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<ErrorDetail>? Errors { get; set; }
}

public class ErrorDetail
{
    public string Field { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
