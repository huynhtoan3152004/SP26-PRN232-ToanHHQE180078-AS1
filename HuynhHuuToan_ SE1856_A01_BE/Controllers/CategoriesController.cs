using HuynhHuuToan__SE1856_A01_BE.Models;
using HuynhHuuToan__SE1856_A01_Service.Common;
using HuynhHuuToan__SE1856_A01_Service.DTOs.Category;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;
using HuynhHuuToan__SE1856_A01_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HuynhHuuToan__SE1856_A01_BE.Controllers;

/// <summary>
/// API Controller cho Category - CRUD operations
/// Route sử dụng danh từ số nhiều theo RESTful convention
/// </summary>
[Route("api/categories")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// GET: api/categories
    /// Lấy danh sách Category với Search, Filter, Sort, Paging, Field Selection
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<CategoryResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories([FromQuery] CategoryQueryParams queryParams)
    {
        // Nếu có Fields parameter, sử dụng Dynamic response
        if (!string.IsNullOrWhiteSpace(queryParams.Fields))
        {
            var dynamicResult = await _categoryService.GetAllDynamicAsync(queryParams);
            return Ok(ApiResponse<DynamicPagedResult>.SuccessResponse(dynamicResult, "Categories retrieved successfully"));
        }

        // Standard response với DTO
        var result = await _categoryService.GetAllAsync(queryParams);
        return Ok(ApiResponse<PagedResult<CategoryResponseDto>>.SuccessResponse(result, "Categories retrieved successfully"));
    }

    /// <summary>
    /// GET: api/categories/{id}
    /// Lấy đầy đủ thông tin Category theo ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
            return NotFound(ApiResponse<object>.NotFoundResponse($"Category with ID {id} not found"));

        return Ok(ApiResponse<CategoryDetailDto>.SuccessResponse(category, "Category retrieved successfully"));
    }

    /// <summary>
    /// POST: api/categories
    /// Tạo mới Category - Yêu cầu đăng nhập
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<CategoryResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto createDto)
    {
        var result = await _categoryService.CreateAsync(createDto);
        var response = ApiResponse<CategoryResponseDto>.SuccessResponse(result, "Category created successfully");
        return CreatedAtAction(nameof(GetCategory), new { id = result.CategoryID }, response);
    }

    /// <summary>
    /// PUT: api/categories/{id}
    /// Cập nhật Category - Yêu cầu đăng nhập
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto updateDto)
    {
        if (id != updateDto.CategoryID)
            return BadRequest(ApiResponse<object>.FailResponse("ID mismatch", new List<string> { $"Route ID: {id}, Body ID: {updateDto.CategoryID}" }));

        var result = await _categoryService.UpdateAsync(updateDto);
        if (!result)
            return NotFound(ApiResponse<object>.NotFoundResponse($"Category with ID {id} not found"));

        return Ok(ApiResponse.SuccessResponse("Category updated successfully"));
    }

    /// <summary>
    /// DELETE: api/categories/{id}
    /// Xóa Category - Yêu cầu đăng nhập
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _categoryService.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<object>.NotFoundResponse($"Category with ID {id} not found"));

        return Ok(ApiResponse.SuccessResponse("Category deleted successfully"));
    }
}
