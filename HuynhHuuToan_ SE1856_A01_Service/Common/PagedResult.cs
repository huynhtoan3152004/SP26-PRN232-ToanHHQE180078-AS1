namespace HuynhHuuToan__SE1856_A01_Service.Common;

/// <summary>
/// Kết quả phân trang - theo Requirement 5
/// Response phải kèm metadata phân trang: page, pageSize, totalItems, totalPages
/// </summary>
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    
    /// <summary>
    /// Số trang hiện tại (đổi từ PageNumber theo requirement)
    /// </summary>
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}

/// <summary>
/// Kết quả phân trang động - cho Fields projection
/// Items là List<object> để chứa ExpandoObject
/// </summary>
public class DynamicPagedResult
{
    public List<object> Items { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}
