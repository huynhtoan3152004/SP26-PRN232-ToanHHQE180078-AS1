using HuynhHuuToan__SE1856_A01_BE.Models;
using HuynhHuuToan__SE1856_A01_Service.Common;
using HuynhHuuToan__SE1856_A01_Service.DTOs.NewsArticle;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;
using HuynhHuuToan__SE1856_A01_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HuynhHuuToan__SE1856_A01_BE.Controllers;

/// <summary>
/// API Controller cho NewsArticle - CRUD operations
/// Route sử dụng danh từ số nhiều theo RESTful convention
/// </summary>
[Route("api/news-articles")]
[ApiController]
public class NewsArticlesController : ControllerBase
{
    private readonly INewsArticleService _newsArticleService;

    public NewsArticlesController(INewsArticleService newsArticleService)
    {
        _newsArticleService = newsArticleService;
    }

    /// <summary>
    /// GET: api/news-articles
    /// Lấy danh sách NewsArticle với Search, Filter, Sort, Paging
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<NewsArticleResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNewsArticles([FromQuery] NewsArticleQueryParams queryParams)
    {
        var result = await _newsArticleService.GetAllAsync(queryParams);
        return Ok(ApiResponse<PagedResult<NewsArticleResponseDto>>.SuccessResponse(result, "News articles retrieved successfully"));
    }

    /// <summary>
    /// GET: api/news-articles/{id}
    /// Lấy đầy đủ thông tin: Category, CreatedBy, UpdatedBy, Tags
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<NewsArticleDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNewsArticle(int id)
    {
        var article = await _newsArticleService.GetByIdAsync(id);
        if (article == null)
            return NotFound(ApiResponse<object>.NotFoundResponse($"NewsArticle with ID {id} not found"));

        return Ok(ApiResponse<NewsArticleDetailDto>.SuccessResponse(article, "News article retrieved successfully"));
    }

    /// <summary>
    /// POST: api/news-articles
    /// Tạo mới NewsArticle - Yêu cầu đăng nhập
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<NewsArticleResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateNewsArticle([FromBody] NewsArticleCreateDto createDto)
    {
        var result = await _newsArticleService.CreateAsync(createDto);
        var response = ApiResponse<NewsArticleResponseDto>.SuccessResponse(result, "News article created successfully");
        return CreatedAtAction(nameof(GetNewsArticle), new { id = result.NewsArticleID }, response);
    }

    /// <summary>
    /// PUT: api/news-articles/{id}
    /// Cập nhật NewsArticle - Yêu cầu đăng nhập
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateNewsArticle(int id, [FromBody] NewsArticleUpdateDto updateDto)
    {
        if (id != updateDto.NewsArticleID)
            return BadRequest(ApiResponse<object>.FailResponse("ID mismatch", new List<string> { $"Route ID: {id}, Body ID: {updateDto.NewsArticleID}" }));

        var result = await _newsArticleService.UpdateAsync(updateDto);
        if (!result)
            return NotFound(ApiResponse<object>.NotFoundResponse($"NewsArticle with ID {id} not found"));

        return Ok(ApiResponse.SuccessResponse("News article updated successfully"));
    }

    /// <summary>
    /// DELETE: api/news-articles/{id}
    /// Xóa NewsArticle - Yêu cầu đăng nhập
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNewsArticle(int id)
    {
        var result = await _newsArticleService.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<object>.NotFoundResponse($"NewsArticle with ID {id} not found"));

        return Ok(ApiResponse.SuccessResponse("News article deleted successfully"));
    }
}
