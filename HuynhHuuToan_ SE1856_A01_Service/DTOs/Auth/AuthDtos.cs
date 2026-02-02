using System.ComponentModel.DataAnnotations;

namespace HuynhHuuToan__SE1856_A01_Service.DTOs.Auth;

/// <summary>
/// Request model cho Login API
/// </summary>
public class LoginRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}

/// <summary>
/// Response model cho Login API - chứa JWT token
/// </summary>
public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public int ExpiresIn { get; set; } // seconds
    public string TokenType { get; set; } = "Bearer";
    public AccountInfoDto Account { get; set; } = null!;
}

/// <summary>
/// Thông tin account trả về sau khi login
/// </summary>
public class AccountInfoDto
{
    public int AccountId { get; set; }
    public string AccountName { get; set; } = null!;
    public string AccountEmail { get; set; } = null!;
    public int AccountRole { get; set; }
    public string RoleName { get; set; } = null!;
}
