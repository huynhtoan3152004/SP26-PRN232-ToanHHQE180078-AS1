using HuynhHuuToan__SE1856_A01_BE.Models;
using HuynhHuuToan__SE1856_A01_Service.DTOs.Auth;
using HuynhHuuToan__SE1856_A01_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace HuynhHuuToan__SE1856_A01_BE.Controllers;

/// <summary>
/// Authentication Controller - JWT Login
/// Theo Requirement 7: JWT (JSON Web Token) là bắt buộc
/// </summary>
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// POST: api/auth/login
    /// Đăng nhập và nhận JWT token
    /// </summary>
    /// <param name="request">Email và Password</param>
    /// <returns>JWT Token và thông tin account</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request.Email, request.Password);

        if (result == null)
        {
            return Unauthorized(ApiResponse<object>.FailResponse("Invalid email or password"));
        }

        return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(result, "Login successful"));
    }
}
