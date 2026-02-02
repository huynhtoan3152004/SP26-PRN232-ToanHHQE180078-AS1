namespace HuynhHuuToan__SE1856_A01_Service.Common;

/// <summary>
/// Base class cho tất cả query parameters - chứa Search, Sort, Paging, Selection, Expansion
/// </summary>
public class BaseQueryParams
{
    // --- SEARCH ---
    public string? SearchTerm { get; set; }

    // --- SORT ---
    public string? SortBy { get; set; }
    public string SortOrder { get; set; } = "asc"; // asc hoặc desc

    // --- PAGING ---
    private int _pageNumber = 1;
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 10 : (value > 100 ? 100 : value);
    }

    // --- SELECTION (chọn fields cần trả về) ---
    // Ví dụ: fields=CategoryID,CategoryName
    public string? Fields { get; set; }

    // --- EXPANSION (include navigation properties) ---
    // Ví dụ: expand=Parent,Children
    public string? Expand { get; set; }

    // --- HELPER METHODS ---
    /// <summary>
    /// Parse Fields string thành List
    /// </summary>
    public List<string> GetFields()
    {
        if (string.IsNullOrWhiteSpace(Fields))
            return new List<string>();

        return Fields.Split(',', StringSplitOptions.RemoveEmptyEntries)
                     .Select(f => f.Trim())
                     .Where(f => !string.IsNullOrWhiteSpace(f))
                     .ToList();
    }

    /// <summary>
    /// Parse Expand string thành List
    /// </summary>
    public List<string> GetExpands()
    {
        if (string.IsNullOrWhiteSpace(Expand))
            return new List<string>();

        return Expand.Split(',', StringSplitOptions.RemoveEmptyEntries)
                     .Select(e => e.Trim())
                     .Where(e => !string.IsNullOrWhiteSpace(e))
                     .ToList();
    }
}
