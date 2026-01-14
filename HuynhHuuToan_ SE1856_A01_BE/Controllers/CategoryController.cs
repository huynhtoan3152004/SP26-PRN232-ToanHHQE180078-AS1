using HuynhHuuToan__SE1856_A01_Service.DTOs.Category;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;
using HuynhHuuToan__SE1856_A01_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace HuynhHuuToan__SE1856_A01_BE.Controllers;

/// <summary>
/// API Controller cho Category - CRUD operations
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// GET: api/Category?searchTerm=tech&sortBy=CategoryName&pageNumber=1&pageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] CategoryQueryParams queryParams)
    {
        var result = await _categoryService.GetAllAsync(queryParams);
        return Ok(result);
    }

    /// <summary>
    /// GET: api/Category/5
    /// Lấy đầy đủ thông tin Category: Parent, Children, NewsArticleCount
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
            return NotFound(new { message = $"Category with ID {id} not found" });

        return Ok(category);
    }

    /// <summary>
    /// POST: api/Category
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto createDto)
    {
        var result = await _categoryService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetCategory), new { id = result.CategoryID }, result);
    }

    /// <summary>
    /// PUT: api/Category/5
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto updateDto)
    {
        if (id != updateDto.CategoryID)
            return BadRequest(new { message = "ID mismatch" });

        var result = await _categoryService.UpdateAsync(updateDto);
        if (!result)
            return NotFound(new { message = $"Category with ID {id} not found" });

        return NoContent();
    }

    /// <summary>
    /// DELETE: api/Category/5
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _categoryService.DeleteAsync(id);
        if (!result)
            return NotFound(new { message = $"Category with ID {id} not found" });

        return NoContent();
    }
}
