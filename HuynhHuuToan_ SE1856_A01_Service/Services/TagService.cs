using HuynhHuuToan__SE1856_A01_Repository.Models.Entities;
using HuynhHuuToan__SE1856_A01_Repository.UnitOfWork;
using HuynhHuuToan__SE1856_A01_Service.Common;
using HuynhHuuToan__SE1856_A01_Service.DTOs.Tag;
using HuynhHuuToan__SE1856_A01_Service.Extensions;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;
using Microsoft.EntityFrameworkCore;

namespace HuynhHuuToan__SE1856_A01_Service.Services;

public class TagService : ITagService
{
    private readonly IUnitOfWork _unitOfWork;

    public TagService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<TagResponseDto>> GetAllAsync(TagQueryParams queryParams)
    {
        var query = _unitOfWork.Tags.Query(asNoTracking: true);

        // SEARCH
        if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
        {
            var searchLower = queryParams.SearchTerm.ToLower();
            query = query.Where(t =>
                t.TagName.ToLower().Contains(searchLower) ||
                (t.Note != null && t.Note.ToLower().Contains(searchLower))
            );
        }

        // Count
        var totalItems = await query.CountAsync();

        // SORT
        var sortBy = string.IsNullOrWhiteSpace(queryParams.SortBy) ? "TagName" : queryParams.SortBy;
        query = query.ApplySorting(sortBy, queryParams.SortOrder);

        // PAGING
        query = query.ApplyPaging(queryParams.PageNumber, queryParams.PageSize);

        var tags = await query.ToListAsync();
        var items = tags.Select(t => new TagResponseDto
        {
            TagID = t.TagID,
            TagName = t.TagName,
            Note = t.Note
        }).ToList();

        return new PagedResult<TagResponseDto>
        {
            Items = items,
            Page = queryParams.PageNumber,
            PageSize = queryParams.PageSize,
            TotalItems = totalItems
        };
    }

    public async Task<TagDetailDto?> GetByIdAsync(int id)
    {
        var tag = await _unitOfWork.Tags
            .Query(asNoTracking: true)
            .Include(t => t.NewsArticles)
            .FirstOrDefaultAsync(t => t.TagID == id);

        if (tag == null)
            return null;

        return new TagDetailDto
        {
            TagID = tag.TagID,
            TagName = tag.TagName,
            Note = tag.Note,
            NewsArticleCount = tag.NewsArticles.Count
        };
    }

    public async Task<TagResponseDto> CreateAsync(TagCreateDto createDto)
    {
        var tag = new Tag
        {
            TagName = createDto.TagName,
            Note = createDto.Note
        };

        await _unitOfWork.Tags.AddAsync(tag);
        await _unitOfWork.SaveChangesAsync();

        return new TagResponseDto
        {
            TagID = tag.TagID,
            TagName = tag.TagName,
            Note = tag.Note
        };
    }

    public async Task<bool> UpdateAsync(TagUpdateDto updateDto)
    {
        var tag = await _unitOfWork.Tags.FindByIdAsync(default, updateDto.TagID);
        if (tag == null)
            return false;

        tag.TagName = updateDto.TagName;
        tag.Note = updateDto.Note;

        _unitOfWork.Tags.Update(tag);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tag = await _unitOfWork.Tags.FindByIdAsync(default, id);
        if (tag == null)
            return false;

        _unitOfWork.Tags.Remove(tag);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
