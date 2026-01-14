namespace HuynhHuuToan__SE1856_A01_Service.DTOs.Tag;

/// <summary>
/// DTO để tạo mới Tag
/// </summary>
public class TagCreateDto
{
    public string TagName { get; set; } = null!;
    public string? Note { get; set; }
}

/// <summary>
/// DTO để cập nhật Tag
/// </summary>
public class TagUpdateDto
{
    public int TagID { get; set; }
    public string TagName { get; set; } = null!;
    public string? Note { get; set; }
}

/// <summary>
/// DTO trả về danh sách Tag
/// </summary>
public class TagResponseDto
{
    public int TagID { get; set; }
    public string TagName { get; set; } = null!;
    public string? Note { get; set; }
}

/// <summary>
/// DTO trả về chi tiết Tag (kèm số lượng NewsArticle)
/// </summary>
public class TagDetailDto
{
    public int TagID { get; set; }
    public string TagName { get; set; } = null!;
    public string? Note { get; set; }
    public int NewsArticleCount { get; set; }
}
