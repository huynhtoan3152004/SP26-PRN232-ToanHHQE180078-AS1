using HuynhHuuToan__SE1856_A01_Service.Common;
using HuynhHuuToan__SE1856_A01_Service.DTOs.NewsArticle;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;

namespace HuynhHuuToan__SE1856_A01_Service.Services;

public interface INewsArticleService
{
    Task<PagedResult<NewsArticleResponseDto>> GetAllAsync(NewsArticleQueryParams queryParams);
    Task<NewsArticleDetailDto?> GetByIdAsync(int id);
    Task<NewsArticleResponseDto> CreateAsync(NewsArticleCreateDto createDto);
    Task<bool> UpdateAsync(NewsArticleUpdateDto updateDto);
    Task<bool> DeleteAsync(int id);
}
