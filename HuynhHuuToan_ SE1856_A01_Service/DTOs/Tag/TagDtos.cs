using System.ComponentModel.DataAnnotations;

namespace HuynhHuuToan__SE1856_A01_Service.DTOs.Tag;

/// <summary>
/// Request Model để tạo mới Tag
/// Validation theo Requirement 8
/// </summary>
public class TagCreateDto
{
    [Required(ErrorMessage = "TagName is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "TagName must be between 2 and 100 characters")]
    public string TagName { get; set; } = null!;

    [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters")]
    public string? Note { get; set; }
}

/// <summary>
/// Request Model để cập nhật Tag
/// </summary>
public class TagUpdateDto
{
    [Required(ErrorMessage = "TagID is required")]
    public int TagID { get; set; }

    [Required(ErrorMessage = "TagName is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "TagName must be between 2 and 100 characters")]
    public string TagName { get; set; } = null!;

    [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters")]
    public string? Note { get; set; }
}

/// <summary>
/// Response Model trả về danh sách Tag
/// </summary>
public class TagResponseDto
{
    public int TagID { get; set; }
    public string TagName { get; set; } = null!;
    public string? Note { get; set; }
}

/// <summary>
/// Response Model trả về chi tiết Tag
/// Theo Requirement 4: Trả về đầy đủ thông tin liên quan của resource
/// </summary>
public class TagDetailDto
{
    public int TagID { get; set; }
    public string TagName { get; set; } = null!;
    public string? Note { get; set; }
    public int NewsArticleCount { get; set; }
}
