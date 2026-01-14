using HuynhHuuToan__SE1856_A01_Service.DTOs.NewsArticle;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;
using HuynhHuuToan__SE1856_A01_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace HuynhHuuToan__SE1856_A01_BE.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsArticleController : ControllerBase
{
    private readonly INewsArticleService _newsArticleService;

    public NewsArticleController(INewsArticleService newsArticleService)
    {
        _newsArticleService = newsArticleService;
    }

    /// <summary>
    /// GET: api/NewsArticle?searchTerm=covid&categoryID=1&newsStatus=true&sortBy=CreatedDate&sortOrder=desc&pageNumber=1&pageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetNewsArticles([FromQuery] NewsArticleQueryParams queryParams)
    {
        var result = await _newsArticleService.GetAllAsync(queryParams);
        return Ok(result);
    }

    /// <summary>
    /// GET: api/NewsArticle/5
    /// Lấy đầy đủ thông tin: Category, CreatedBy, UpdatedBy, Tags
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNewsArticle(int id)
    {
        var article = await _newsArticleService.GetByIdAsync(id);
        if (article == null)
            return NotFound(new { message = $"NewsArticle with ID {id} not found" });

        return Ok(article);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewsArticle([FromBody] NewsArticleCreateDto createDto)
    {
        var result = await _newsArticleService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetNewsArticle), new { id = result.NewsArticleID }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNewsArticle(int id, [FromBody] NewsArticleUpdateDto updateDto)
    {
        if (id != updateDto.NewsArticleID)
            return BadRequest(new { message = "ID mismatch" });

        var result = await _newsArticleService.UpdateAsync(updateDto);
        if (!result)
            return NotFound(new { message = $"NewsArticle with ID {id} not found" });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNewsArticle(int id)
    {
        var result = await _newsArticleService.DeleteAsync(id);
        if (!result)
            return NotFound(new { message = $"NewsArticle with ID {id} not found" });

        return NoContent();
    }
}
