namespace HuynhHuuToan__SE1856_A01_Service.DTOs.SystemAccount;

/// <summary>
/// DTO để tạo mới SystemAccount
/// </summary>
public class SystemAccountCreateDto
{
    public string AccountName { get; set; } = null!;
    public string AccountEmail { get; set; } = null!;
    public int AccountRole { get; set; }
    public string AccountPassword { get; set; } = null!;
}

/// <summary>
/// DTO để cập nhật SystemAccount
/// </summary>
public class SystemAccountUpdateDto
{
    public int AccountID { get; set; }
    public string AccountName { get; set; } = null!;
    public string AccountEmail { get; set; } = null!;
    public int AccountRole { get; set; }
    public string AccountPassword { get; set; } = null!;
}

/// <summary>
/// DTO trả về danh sách SystemAccount (không trả password)
/// </summary>
public class SystemAccountResponseDto
{
    public int AccountID { get; set; }
    public string AccountName { get; set; } = null!;
    public string AccountEmail { get; set; } = null!;
    public int AccountRole { get; set; }
}

/// <summary>
/// DTO trả về chi tiết SystemAccount (kèm số lượng NewsArticle đã tạo/cập nhật)
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
