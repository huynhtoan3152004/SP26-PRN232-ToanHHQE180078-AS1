using HuynhHuuToan__SE1856_A01_Service.DTOs.Tag;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;
using HuynhHuuToan__SE1856_A01_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace HuynhHuuToan__SE1856_A01_BE.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTags([FromQuery] TagQueryParams queryParams)
    {
        var result = await _tagService.GetAllAsync(queryParams);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTag(int id)
    {
        var tag = await _tagService.GetByIdAsync(id);
        if (tag == null)
            return NotFound(new { message = $"Tag with ID {id} not found" });

        return Ok(tag);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] TagCreateDto createDto)
    {
        var result = await _tagService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetTag), new { id = result.TagID }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTag(int id, [FromBody] TagUpdateDto updateDto)
    {
        if (id != updateDto.TagID)
            return BadRequest(new { message = "ID mismatch" });

        var result = await _tagService.UpdateAsync(updateDto);
        if (!result)
            return NotFound(new { message = $"Tag with ID {id} not found" });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(int id)
    {
        var result = await _tagService.DeleteAsync(id);
        if (!result)
            return NotFound(new { message = $"Tag with ID {id} not found" });

        return NoContent();
    }
}
