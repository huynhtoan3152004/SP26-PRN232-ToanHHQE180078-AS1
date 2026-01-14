using HuynhHuuToan__SE1856_A01_Service.Common;

namespace HuynhHuuToan__SE1856_A01_Service.QueryParams;

/// <summary>
/// Query parameters cho NewsArticle
/// </summary>
public class NewsArticleQueryParams : BaseQueryParams
{
    // Filter theo Category
    public int? CategoryID { get; set; }

    // Filter theo Status
    public bool? NewsStatus { get; set; }

    // Filter theo CreatedBy
    public int? CreatedByID { get; set; }

    // Filter theo ngày tạo
    public DateTime? CreatedDateFrom { get; set; }
    public DateTime? CreatedDateTo { get; set; }
}
