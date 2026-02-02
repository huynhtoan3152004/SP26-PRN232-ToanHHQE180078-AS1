using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HuynhHuuToan__SE1856_A01_Repository.UnitOfWork;
using HuynhHuuToan__SE1856_A01_Service.DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HuynhHuuToan__SE1856_A01_Service.Services;

/// <summary>
/// Authentication Service - JWT Token Generation
/// Theo Requirement 7: JWT (JSON Web Token) là bắt buộc
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    /// <summary>
    /// Xác thực user và tạo JWT token
    /// </summary>
    public async Task<LoginResponseDto?> LoginAsync(string email, string password)
    {
        // Tìm account theo email
        var account = await _unitOfWork.SystemAccounts
            .Query(asNoTracking: true)
            .FirstOrDefaultAsync(a => a.AccountEmail.ToLower() == email.ToLower());

        if (account == null)
            return null;

        // Kiểm tra password (plaintext comparison - trong production nên hash)
        if (account.AccountPassword != password)
            return null;

        // Tạo JWT token
        var token = GenerateJwtToken(account.AccountID, account.AccountEmail, account.AccountRole);
        var expiryMinutes = int.Parse(_configuration["JwtSettings:ExpiryMinutes"] ?? "60");

        return new LoginResponseDto
        {
            Token = token,
            ExpiresIn = expiryMinutes * 60, // convert to seconds
            TokenType = "Bearer",
            Account = new AccountInfoDto
            {
                AccountId = account.AccountID,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = account.AccountRole,
                RoleName = GetRoleName(account.AccountRole)
            }
        };
    }

    /// <summary>
    /// Tạo JWT token
    /// </summary>
    private string GenerateJwtToken(int userId, string email, int role)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"] 
            ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = _configuration["JwtSettings:Issuer"] ?? "FUNewsManagement";
        var audience = _configuration["JwtSettings:Audience"] ?? "FUNewsManagementClient";
        var expiryMinutes = int.Parse(_configuration["JwtSettings:ExpiryMinutes"] ?? "60");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, GetRoleName(role)),
            new Claim("role_id", role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Lấy role name từ role ID
    /// Role: 1 = Staff, 2 = Lecturer, 3 = Admin (dựa trên FU News Management System)
    /// </summary>
    public string GetRoleName(int roleId)
    {
        return roleId switch
        {
            1 => "Staff",
            2 => "Lecturer",
            3 => "Admin",
            _ => "Unknown"
        };
    }
}
