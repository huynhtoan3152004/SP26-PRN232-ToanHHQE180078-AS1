using HuynhHuuToan__SE1856_A01_Service.DTOs.Auth;

namespace HuynhHuuToan__SE1856_A01_Service.Services;

/// <summary>
/// Interface cho Authentication Service
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Xác thực user và trả về JWT token
    /// </summary>
    Task<LoginResponseDto?> LoginAsync(string email, string password);
    
    /// <summary>
    /// Lấy role name từ role ID
    /// </summary>
    string GetRoleName(int roleId);
}
