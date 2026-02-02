using System.Net;
using System.Text.Json;
using HuynhHuuToan__SE1856_A01_BE.Models;

namespace HuynhHuuToan__SE1856_A01_BE.Middleware;

/// <summary>
/// Global Exception Handler Middleware - theo Requirement 8
/// - Xử lý exception bằng Global Exception Handler
/// - Không expose stack trace hoặc thông tin nội bộ
/// - Trả response lỗi theo format chuẩn
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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

        var response = new ApiResponse<object>();
        
        switch (exception)
        {
            case UnauthorizedAccessException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response = ApiResponse<object>.FailResponse("Unauthorized access");
                break;

            case KeyNotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response = ApiResponse<object>.FailResponse(exception.Message);
                break;

            case ArgumentException:
            case InvalidOperationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = ApiResponse<object>.FailResponse(exception.Message);
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                // Không expose stack trace - chỉ trả message chung
                response = ApiResponse<object>.FailResponse("An internal server error occurred");
                break;
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);
        await context.Response.WriteAsync(jsonResponse);
    }
}

/// <summary>
/// Extension method để đăng ký middleware
/// </summary>
public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
