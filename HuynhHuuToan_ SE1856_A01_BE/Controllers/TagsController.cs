using HuynhHuuToan__SE1856_A01_BE.Models;
using HuynhHuuToan__SE1856_A01_Service.Common;
using HuynhHuuToan__SE1856_A01_Service.DTOs.Tag;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;
using HuynhHuuToan__SE1856_A01_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HuynhHuuToan__SE1856_A01_BE.Controllers;

/// <summary>
/// API Controller cho Tag - CRUD operations
/// Route sử dụng danh từ số nhiều theo RESTful convention
/// </summary>
[Route("api/tags")]
[ApiController]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    /// <summary>
    /// GET: api/tags
    /// Lấy danh sách Tag với Search, Filter, Sort, Paging
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<TagResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTags([FromQuery] TagQueryParams queryParams)
    {
        var result = await _tagService.GetAllAsync(queryParams);
        return Ok(ApiResponse<PagedResult<TagResponseDto>>.SuccessResponse(result, "Tags retrieved successfully"));
    }

    /// <summary>
    /// GET: api/tags/{id}
    /// Lấy chi tiết Tag theo ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<TagDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTag(int id)
    {
        var tag = await _tagService.GetByIdAsync(id);
        if (tag == null)
            return NotFound(ApiResponse<object>.NotFoundResponse($"Tag with ID {id} not found"));

        return Ok(ApiResponse<TagDetailDto>.SuccessResponse(tag, "Tag retrieved successfully"));
    }

    /// <summary>
    /// POST: api/tags
    /// Tạo mới Tag - Yêu cầu đăng nhập
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<TagResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateTag([FromBody] TagCreateDto createDto)
    {
        var result = await _tagService.CreateAsync(createDto);
        var response = ApiResponse<TagResponseDto>.SuccessResponse(result, "Tag created successfully");
        return CreatedAtAction(nameof(GetTag), new { id = result.TagID }, response);
    }

    /// <summary>
    /// PUT: api/tags/{id}
    /// Cập nhật Tag - Yêu cầu đăng nhập
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTag(int id, [FromBody] TagUpdateDto updateDto)
    {
        if (id != updateDto.TagID)
            return BadRequest(ApiResponse<object>.FailResponse("ID mismatch", new List<string> { $"Route ID: {id}, Body ID: {updateDto.TagID}" }));

        var result = await _tagService.UpdateAsync(updateDto);
        if (!result)
            return NotFound(ApiResponse<object>.NotFoundResponse($"Tag with ID {id} not found"));

        return Ok(ApiResponse.SuccessResponse("Tag updated successfully"));
    }

    /// <summary>
    /// DELETE: api/tags/{id}
    /// Xóa Tag - Yêu cầu đăng nhập
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTag(int id)
    {
        var result = await _tagService.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<object>.NotFoundResponse($"Tag with ID {id} not found"));

        return Ok(ApiResponse.SuccessResponse("Tag deleted successfully"));
    }
}
