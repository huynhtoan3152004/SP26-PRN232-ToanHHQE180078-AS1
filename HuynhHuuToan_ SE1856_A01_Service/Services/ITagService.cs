using HuynhHuuToan__SE1856_A01_Service.Common;
using HuynhHuuToan__SE1856_A01_Service.DTOs.Tag;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;

namespace HuynhHuuToan__SE1856_A01_Service.Services;

public interface ITagService
{
    Task<PagedResult<TagResponseDto>> GetAllAsync(TagQueryParams queryParams);
    Task<TagDetailDto?> GetByIdAsync(int id);
    Task<TagResponseDto> CreateAsync(TagCreateDto createDto);
    Task<bool> UpdateAsync(TagUpdateDto updateDto);
    Task<bool> DeleteAsync(int id);
}
