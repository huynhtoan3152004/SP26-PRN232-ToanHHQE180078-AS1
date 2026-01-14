using HuynhHuuToan__SE1856_A01_Service.Common;

namespace HuynhHuuToan__SE1856_A01_Service.QueryParams;

/// <summary>
/// Query parameters cho Category - kế thừa BaseQueryParams
/// Thêm các filter riêng cho Category
/// </summary>
public class CategoryQueryParams : BaseQueryParams
{
    // Filter theo Parent Category
    public int? ParentCategoryID { get; set; }

    // Filter theo trạng thái Active
    public bool? IsActive { get; set; }
}
