namespace HuynhHuuToan__SE1856_A01_BE.Models;

/// <summary>
/// Standard API Response wrapper - theo Requirement 6
/// Response phải bao gồm: success, message, data, errors
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    // Factory methods để tạo response nhanh
    public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Errors = null
        };
    }

    public static ApiResponse<T> FailResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default,
            Errors = errors
        };
    }

    public static ApiResponse<T> NotFoundResponse(string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default,
            Errors = null
        };
    }
}

/// <summary>
/// Non-generic version for responses without data
/// </summary>
public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse SuccessResponse(string message = "Success")
    {
        return new ApiResponse
        {
            Success = true,
            Message = message,
            Data = null,
            Errors = null
        };
    }

    public new static ApiResponse FailResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            Data = null,
            Errors = errors
        };
    }
}
