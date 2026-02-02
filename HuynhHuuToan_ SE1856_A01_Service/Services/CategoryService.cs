using HuynhHuuToan__SE1856_A01_Repository.Models.Entities;
using HuynhHuuToan__SE1856_A01_Repository.UnitOfWork;
using HuynhHuuToan__SE1856_A01_Service.Common;
using HuynhHuuToan__SE1856_A01_Service.DTOs.Category;
using HuynhHuuToan__SE1856_A01_Service.Extensions;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;
using Microsoft.EntityFrameworkCore;

namespace HuynhHuuToan__SE1856_A01_Service.Services;

/// <summary>
/// Implementation CategoryService - sử dụng Unit of Work pattern
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Lấy danh sách Category với Search, Sort, Paging, Filter
    /// </summary>
    public async Task<PagedResult<CategoryResponseDto>> GetAllAsync(CategoryQueryParams queryParams)
    {
        // Bước 1: Lấy IQueryable từ Repository
        var query = _unitOfWork.Categories.Query(asNoTracking: true);

        // 🆕 EXPAND: Eager loading dựa trên queryParams.Expand
        var expands = queryParams.GetExpands();
        foreach (var expand in expands)
        {
            switch (expand.ToLower())
            {
                case "parentcategory":
                case "parent":
                    query = query.Include(c => c.ParentCategory);
                    break;
                case "children":
                case "inverseparentcategory":
                    query = query.Include(c => c.InverseParentCategory);
                    break;
                case "newsarticles":
                    query = query.Include(c => c.NewsArticles);
                    break;
            }
        }

        // Bước 2: Apply SEARCH (tìm kiếm theo CategoryName hoặc Description)
        if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
        {
            var searchLower = queryParams.SearchTerm.ToLower();
            query = query.Where(c =>
                c.CategoryName.ToLower().Contains(searchLower) ||
                (c.CategoryDescription != null && c.CategoryDescription.ToLower().Contains(searchLower))
            );
        }

        // Bước 3: Apply FILTER riêng của Category
        if (queryParams.ParentCategoryID.HasValue)
        {
            query = query.Where(c => c.ParentCategoryID == queryParams.ParentCategoryID.Value);
        }

        if (queryParams.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == queryParams.IsActive.Value);
        }

        // Bước 4: Đếm tổng số record (trước khi paging)
        var totalItems = await query.CountAsync();

        // Bước 5: Apply SORTING (mặc định sort theo CategoryName)
        var sortBy = string.IsNullOrWhiteSpace(queryParams.SortBy) ? "CategoryName" : queryParams.SortBy;
        query = query.ApplySorting(sortBy, queryParams.SortOrder);

        // Bước 6: Apply PAGING
        query = query.ApplyPaging(queryParams.PageNumber, queryParams.PageSize);

        // Bước 7: Thực thi query và map sang DTO
        var categories = await query.ToListAsync();
        var items = categories.Select(c => new CategoryResponseDto
        {
            CategoryID = c.CategoryID,
            CategoryName = c.CategoryName,
            CategoryDescription = c.CategoryDescription,
            ParentCategoryID = c.ParentCategoryID,
            IsActive = c.IsActive
        }).ToList();

        // Bước 8: Trả về PagedResult
        return new PagedResult<CategoryResponseDto>
        {
            Items = items,
            Page = queryParams.PageNumber,
            PageSize = queryParams.PageSize,
            TotalItems = totalItems
        };
    }

    /// <summary>
    /// 🆕 Lấy danh sách Category với Fields projection (Dynamic response)
    /// </summary>
    public async Task<DynamicPagedResult> GetAllDynamicAsync(CategoryQueryParams queryParams)
    {
        // Bước 1: Lấy IQueryable từ Repository
        var query = _unitOfWork.Categories.Query(asNoTracking: true);

        // EXPAND: Eager loading
        var expands = queryParams.GetExpands();
        foreach (var expand in expands)
        {
            switch (expand.ToLower())
            {
                case "parentcategory":
                case "parent":
                    query = query.Include(c => c.ParentCategory);
                    break;
                case "children":
                case "inverseparentcategory":
                    query = query.Include(c => c.InverseParentCategory);
                    break;
            }
        }

        // SEARCH
        if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
        {
            var searchLower = queryParams.SearchTerm.ToLower();
            query = query.Where(c =>
                c.CategoryName.ToLower().Contains(searchLower) ||
                (c.CategoryDescription != null && c.CategoryDescription.ToLower().Contains(searchLower))
            );
        }

        // FILTER
        if (queryParams.ParentCategoryID.HasValue)
            query = query.Where(c => c.ParentCategoryID == queryParams.ParentCategoryID.Value);

        if (queryParams.IsActive.HasValue)
            query = query.Where(c => c.IsActive == queryParams.IsActive.Value);

        // Total count
        var totalItems = await query.CountAsync();

        // SORTING
        var sortBy = string.IsNullOrWhiteSpace(queryParams.SortBy) ? "CategoryName" : queryParams.SortBy;
        query = query.ApplySorting(sortBy, queryParams.SortOrder);

        // PAGING
        query = query.ApplyPaging(queryParams.PageNumber, queryParams.PageSize);

        // Execute query
        var categories = await query.ToListAsync();
        var items = categories.Select(c => new CategoryResponseDto
        {
            CategoryID = c.CategoryID,
            CategoryName = c.CategoryName,
            CategoryDescription = c.CategoryDescription,
            ParentCategoryID = c.ParentCategoryID,
            IsActive = c.IsActive
        }).ToList();

        // 🆕 FIELDS: Shape data theo fields được chọn
        var fields = queryParams.GetFields();
        var shapedData = DynamicResponseHelper.ShapeData(items, fields);

        return new DynamicPagedResult
        {
            Items = shapedData.Cast<object>().ToList(),
            Page = queryParams.PageNumber,
            PageSize = queryParams.PageSize,
            TotalItems = totalItems
        };
    }

    /// <summary>
    /// Lấy chi tiết Category theo ID - ĐẦY ĐỦ thông tin (Parent, Children, NewsCount)
    /// </summary>
    public async Task<CategoryDetailDto?> GetByIdAsync(int id)
    {
        // Include Parent và Children để lấy đầy đủ thông tin
        var category = await _unitOfWork.Categories
            .Query(asNoTracking: true)
            .Include(c => c.ParentCategory)
            .Include(c => c.InverseParentCategory) // Children
            .Include(c => c.NewsArticles)
            .FirstOrDefaultAsync(c => c.CategoryID == id);

        if (category == null)
            return null;

        // Map sang DetailDto với đầy đủ thông tin
        return new CategoryDetailDto
        {
            CategoryID = category.CategoryID,
            CategoryName = category.CategoryName,
            CategoryDescription = category.CategoryDescription,
            ParentCategoryID = category.ParentCategoryID,
            IsActive = category.IsActive,
            
            // Parent Category
            ParentCategory = category.ParentCategory == null ? null : new CategoryResponseDto
            {
                CategoryID = category.ParentCategory.CategoryID,
                CategoryName = category.ParentCategory.CategoryName,
                CategoryDescription = category.ParentCategory.CategoryDescription,
                ParentCategoryID = category.ParentCategory.ParentCategoryID,
                IsActive = category.ParentCategory.IsActive
            },
            
            // Children
            Children = category.InverseParentCategory.Select(child => new CategoryResponseDto
            {
                CategoryID = child.CategoryID,
                CategoryName = child.CategoryName,
                CategoryDescription = child.CategoryDescription,
                ParentCategoryID = child.ParentCategoryID,
                IsActive = child.IsActive
            }).ToList(),
            
            // Số lượng NewsArticle
            NewsArticleCount = category.NewsArticles.Count
        };
    }

    /// <summary>
    /// Tạo mới Category
    /// </summary>
    public async Task<CategoryResponseDto> CreateAsync(CategoryCreateDto createDto)
    {
        var category = new Category
        {
            CategoryName = createDto.CategoryName,
            CategoryDescription = createDto.CategoryDescription,
            ParentCategoryID = createDto.ParentCategoryID,
            IsActive = createDto.IsActive
        };

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return new CategoryResponseDto
        {
            CategoryID = category.CategoryID,
            CategoryName = category.CategoryName,
            CategoryDescription = category.CategoryDescription,
            ParentCategoryID = category.ParentCategoryID,
            IsActive = category.IsActive
        };
    }

    /// <summary>
    /// Cập nhật Category
    /// </summary>
    public async Task<bool> UpdateAsync(CategoryUpdateDto updateDto)
    {
        var category = await _unitOfWork.Categories.FindByIdAsync(default, updateDto.CategoryID);
        if (category == null)
            return false;

        category.CategoryName = updateDto.CategoryName;
        category.CategoryDescription = updateDto.CategoryDescription;
        category.ParentCategoryID = updateDto.ParentCategoryID;
        category.IsActive = updateDto.IsActive;

        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Xóa Category
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _unitOfWork.Categories.FindByIdAsync(default, id);
        if (category == null)
            return false;

        _unitOfWork.Categories.Remove(category);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
