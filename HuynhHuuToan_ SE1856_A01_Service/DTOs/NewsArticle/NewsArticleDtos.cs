using System.ComponentModel.DataAnnotations;

namespace HuynhHuuToan__SE1856_A01_Service.DTOs.NewsArticle;

/// <summary>
/// Request Model để tạo mới NewsArticle
/// Validation theo Requirement 8
/// </summary>
public class NewsArticleCreateDto
{
    [Required(ErrorMessage = "NewsTitle is required")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "NewsTitle must be between 5 and 200 characters")]
    public string NewsTitle { get; set; } = null!;

    [StringLength(300, ErrorMessage = "Headline cannot exceed 300 characters")]
    public string? Headline { get; set; }

    [Required(ErrorMessage = "NewsContent is required")]
    public string NewsContent { get; set; } = null!;

    [StringLength(200, ErrorMessage = "NewsSource cannot exceed 200 characters")]
    public string? NewsSource { get; set; }

    [Required(ErrorMessage = "CategoryID is required")]
    public int CategoryID { get; set; }

    public bool NewsStatus { get; set; } = true;

    [Required(ErrorMessage = "CreatedByID is required")]
    public int CreatedByID { get; set; }

    public List<int> TagIDs { get; set; } = new();
}

/// <summary>
/// Request Model để cập nhật NewsArticle
/// </summary>
public class NewsArticleUpdateDto
{
    [Required(ErrorMessage = "NewsArticleID is required")]
    public int NewsArticleID { get; set; }

    [Required(ErrorMessage = "NewsTitle is required")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "NewsTitle must be between 5 and 200 characters")]
    public string NewsTitle { get; set; } = null!;

    [StringLength(300, ErrorMessage = "Headline cannot exceed 300 characters")]
    public string? Headline { get; set; }

    [Required(ErrorMessage = "NewsContent is required")]
    public string NewsContent { get; set; } = null!;

    [StringLength(200, ErrorMessage = "NewsSource cannot exceed 200 characters")]
    public string? NewsSource { get; set; }

    [Required(ErrorMessage = "CategoryID is required")]
    public int CategoryID { get; set; }

    public bool NewsStatus { get; set; }

    public int? UpdatedByID { get; set; }

    public List<int> TagIDs { get; set; } = new();
}

/// <summary>
/// Response Model trả về danh sách NewsArticle (không có NewsContent để giảm tải)
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
/// Response Model trả về chi tiết NewsArticle
/// Theo Requirement 4: Trả về đầy đủ thông tin liên quan của resource
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

    // Navigation properties - 1 cấp, không đệ quy vô hạn
    public CategoryInfo? Category { get; set; }
    public AccountInfo? CreatedBy { get; set; }
    public AccountInfo? UpdatedBy { get; set; }
    public List<TagInfo> Tags { get; set; } = new();

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
