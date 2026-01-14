namespace HuynhHuuToan__SE1856_A01_Service.DTOs.NewsArticle;

/// <summary>
/// DTO để tạo mới NewsArticle
/// </summary>
public class NewsArticleCreateDto
{
    public string NewsTitle { get; set; } = null!;
    public string? Headline { get; set; }
    public string NewsContent { get; set; } = null!;
    public string? NewsSource { get; set; }
    public int CategoryID { get; set; }
    public bool NewsStatus { get; set; } = true;
    public int CreatedByID { get; set; }
    public List<int> TagIDs { get; set; } = new(); // Danh sách TagID để gắn vào NewsArticle
}

/// <summary>
/// DTO để cập nhật NewsArticle
/// </summary>
public class NewsArticleUpdateDto
{
    public int NewsArticleID { get; set; }
    public string NewsTitle { get; set; } = null!;
    public string? Headline { get; set; }
    public string NewsContent { get; set; } = null!;
    public string? NewsSource { get; set; }
    public int CategoryID { get; set; }
    public bool NewsStatus { get; set; }
    public int? UpdatedByID { get; set; }
    public List<int> TagIDs { get; set; } = new();
}

/// <summary>
/// DTO trả về danh sách NewsArticle (không có NewsContent để giảm tải)
/// </summary>
public class NewsArticleResponseDto
{
    public int NewsArticleID { get; set; }
    public string NewsTitle { get; set; } = null!;
    public string? Headline { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? NewsSource { get; set; }
    public int CategoryID { get; set; }
    public string? CategoryName { get; set; }
    public bool NewsStatus { get; set; }
    public int CreatedByID { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

/// <summary>
/// DTO trả về chi tiết NewsArticle (đầy đủ thông tin kèm Category, CreatedBy, UpdatedBy, Tags)
/// </summary>
public class NewsArticleDetailDto
{
    public int NewsArticleID { get; set; }
    public string NewsTitle { get; set; } = null!;
    public string? Headline { get; set; }
    public DateTime CreatedDate { get; set; }
    public string NewsContent { get; set; } = null!;
    public string? NewsSource { get; set; }
    public int CategoryID { get; set; }
    public bool NewsStatus { get; set; }
    public int CreatedByID { get; set; }
    public int? UpdatedByID { get; set; }
    public DateTime? ModifiedDate { get; set; }

    // Navigation properties (đầy đủ thông tin)
    public CategoryInfo? Category { get; set; }
    public AccountInfo? CreatedBy { get; set; }
    public AccountInfo? UpdatedBy { get; set; }
    public List<TagInfo> Tags { get; set; } = new();

    // Nested classes để giữ structure rõ ràng
    public class CategoryInfo
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = null!;
    }

    public class AccountInfo
    {
        public int AccountID { get; set; }
        public string AccountName { get; set; } = null!;
        public string AccountEmail { get; set; } = null!;
    }

    public class TagInfo
    {
        public int TagID { get; set; }
        public string TagName { get; set; } = null!;
    }
}
