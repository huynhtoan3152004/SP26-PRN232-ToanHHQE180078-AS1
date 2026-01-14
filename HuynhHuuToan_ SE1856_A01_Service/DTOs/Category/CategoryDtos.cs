namespace HuynhHuuToan__SE1856_A01_Service.DTOs.Category;

/// <summary>
/// DTO để tạo mới Category
/// </summary>
public class CategoryCreateDto
{
    public string CategoryName { get; set; } = null!;
    public string? CategoryDescription { get; set; }
    public int? ParentCategoryID { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// DTO để cập nhật Category
/// </summary>
public class CategoryUpdateDto
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? CategoryDescription { get; set; }
    public int? ParentCategoryID { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO trả về danh sách Category (đơn giản, không include)
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
/// DTO trả về chi tiết Category (đầy đủ thông tin với Parent và Children)
/// </summary>
public class CategoryDetailDto
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? CategoryDescription { get; set; }
    public int? ParentCategoryID { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties
    public CategoryResponseDto? ParentCategory { get; set; }
    public List<CategoryResponseDto> Children { get; set; } = new();
    public int NewsArticleCount { get; set; }
}
