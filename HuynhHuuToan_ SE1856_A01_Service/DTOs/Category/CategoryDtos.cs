using System.ComponentModel.DataAnnotations;

namespace HuynhHuuToan__SE1856_A01_Service.DTOs.Category;

/// <summary>
/// Request Model để tạo mới Category
/// Validation theo Requirement 8
/// </summary>
public class CategoryCreateDto
{
    [Required(ErrorMessage = "CategoryName is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "CategoryName must be between 2 and 100 characters")]
    public string CategoryName { get; set; } = null!;

    [StringLength(500, ErrorMessage = "CategoryDescription cannot exceed 500 characters")]
    public string? CategoryDescription { get; set; }

    public int? ParentCategoryID { get; set; }

    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Request Model để cập nhật Category
/// </summary>
public class CategoryUpdateDto
{
    [Required(ErrorMessage = "CategoryID is required")]
    public int CategoryID { get; set; }

    [Required(ErrorMessage = "CategoryName is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "CategoryName must be between 2 and 100 characters")]
    public string CategoryName { get; set; } = null!;

    [StringLength(500, ErrorMessage = "CategoryDescription cannot exceed 500 characters")]
    public string? CategoryDescription { get; set; }

    public int? ParentCategoryID { get; set; }

    public bool IsActive { get; set; }
}

/// <summary>
/// Response Model trả về danh sách Category
/// </summary>
public class CategoryResponseDto
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? CategoryDescription { get; set; }
    public int? ParentCategoryID { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// Response Model trả về chi tiết Category (đầy đủ thông tin với Parent và Children)
/// Theo Requirement 4: Trả về đầy đủ thông tin liên quan của resource
/// </summary>
public class CategoryDetailDto
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? CategoryDescription { get; set; }
    public int? ParentCategoryID { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties - 1 cấp, không đệ quy vô hạn
    public CategoryResponseDto? ParentCategory { get; set; }
    public List<CategoryResponseDto> Children { get; set; } = new();
    public int NewsArticleCount { get; set; }
}
