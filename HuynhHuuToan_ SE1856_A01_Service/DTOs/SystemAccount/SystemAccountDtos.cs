using System.ComponentModel.DataAnnotations;

namespace HuynhHuuToan__SE1856_A01_Service.DTOs.SystemAccount;

/// <summary>
/// Request Model để tạo mới SystemAccount
/// Validation theo Requirement 8
/// </summary>
public class SystemAccountCreateDto
{
    [Required(ErrorMessage = "AccountName is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "AccountName must be between 2 and 100 characters")]
    public string AccountName { get; set; } = null!;

    [Required(ErrorMessage = "AccountEmail is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(150, ErrorMessage = "AccountEmail cannot exceed 150 characters")]
    public string AccountEmail { get; set; } = null!;

    [Required(ErrorMessage = "AccountRole is required")]
    [Range(1, 3, ErrorMessage = "AccountRole must be 1 (Staff), 2 (Lecturer), or 3 (Admin)")]
    public int AccountRole { get; set; }

    [Required(ErrorMessage = "AccountPassword is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "AccountPassword must be between 6 and 100 characters")]
    public string AccountPassword { get; set; } = null!;
}

/// <summary>
/// Request Model để cập nhật SystemAccount
/// </summary>
public class SystemAccountUpdateDto
{
    [Required(ErrorMessage = "AccountID is required")]
    public int AccountID { get; set; }

    [Required(ErrorMessage = "AccountName is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "AccountName must be between 2 and 100 characters")]
    public string AccountName { get; set; } = null!;

    [Required(ErrorMessage = "AccountEmail is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(150, ErrorMessage = "AccountEmail cannot exceed 150 characters")]
    public string AccountEmail { get; set; } = null!;

    [Required(ErrorMessage = "AccountRole is required")]
    [Range(1, 3, ErrorMessage = "AccountRole must be 1 (Staff), 2 (Lecturer), or 3 (Admin)")]
    public int AccountRole { get; set; }

    [Required(ErrorMessage = "AccountPassword is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "AccountPassword must be between 6 and 100 characters")]
    public string AccountPassword { get; set; } = null!;
}

/// <summary>
/// Response Model trả về danh sách SystemAccount (không trả password)
/// </summary>
public class SystemAccountResponseDto
{
    public int AccountID { get; set; }
    public string AccountName { get; set; } = null!;
    public string AccountEmail { get; set; } = null!;
    public int AccountRole { get; set; }
}

/// <summary>
/// Response Model trả về chi tiết SystemAccount
/// Theo Requirement 4: Trả về đầy đủ thông tin liên quan của resource
/// </summary>
public class SystemAccountDetailDto
{
    public int AccountID { get; set; }
    public string AccountName { get; set; } = null!;
    public string AccountEmail { get; set; } = null!;
    public int AccountRole { get; set; }
    public int CreatedNewsCount { get; set; }
    public int UpdatedNewsCount { get; set; }
}
