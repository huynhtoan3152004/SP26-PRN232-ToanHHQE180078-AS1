using HuynhHuuToan__SE1856_A01_Service.Common;
using HuynhHuuToan__SE1856_A01_Service.DTOs.Category;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;

namespace HuynhHuuToan__SE1856_A01_Service.Services;

/// <summary>
/// Interface cho CategoryService - định nghĩa các phương thức CRUD
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Lấy danh sách Category với Search, Sort, Paging
    /// </summary>
    Task<PagedResult<CategoryResponseDto>> GetAllAsync(CategoryQueryParams queryParams);

    /// <summary>
    /// Lấy chi tiết Category theo ID (đầy đủ thông tin: Parent, Children, NewsCount)
    /// </summary>
    Task<CategoryDetailDto?> GetByIdAsync(int id);

    /// <summary>
    /// Tạo mới Category
    /// </summary>
    Task<CategoryResponseDto> CreateAsync(CategoryCreateDto createDto);

    /// <summary>
    /// Cập nhật Category
    /// </summary>
    Task<bool> UpdateAsync(CategoryUpdateDto updateDto);

    /// <summary>
    /// Xóa Category
    /// </summary>
    Task<bool> DeleteAsync(int id);
}
