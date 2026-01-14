using HuynhHuuToan__SE1856_A01_Service.Common;

namespace HuynhHuuToan__SE1856_A01_Service.QueryParams;

/// <summary>
/// Query parameters cho SystemAccount
/// </summary>
public class SystemAccountQueryParams : BaseQueryParams
{
    // Filter theo Role
    public int? AccountRole { get; set; }
}
