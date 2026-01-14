using HuynhHuuToan__SE1856_A01_Repository.Models.Entities;
using HuynhHuuToan__SE1856_A01_Repository.UnitOfWork;
using HuynhHuuToan__SE1856_A01_Service.Common;
using HuynhHuuToan__SE1856_A01_Service.DTOs.NewsArticle;
using HuynhHuuToan__SE1856_A01_Service.Extensions;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;
using Microsoft.EntityFrameworkCore;

namespace HuynhHuuToan__SE1856_A01_Service.Services;

public class NewsArticleService : INewsArticleService
{
    private readonly IUnitOfWork _unitOfWork;

    public NewsArticleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<NewsArticleResponseDto>> GetAllAsync(NewsArticleQueryParams queryParams)
    {
        // Bắt đầu với query base
        IQueryable<NewsArticle> query = _unitOfWork.NewsArticles.Query(asNoTracking: true);
        
        // Include navigation properties
        query = query
            .Include(n => n.Category)
            .Include(n => n.CreatedBy);

        // SEARCH
        if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
        {
            var searchLower = queryParams.SearchTerm.ToLower();
            query = query.Where(n =>
                n.NewsTitle.ToLower().Contains(searchLower) ||
                (n.Headline != null && n.Headline.ToLower().Contains(searchLower)) ||
                (n.NewsSource != null && n.NewsSource.ToLower().Contains(searchLower))
            );
        }

        // FILTER by Category
        if (queryParams.CategoryID.HasValue)
        {
            query = query.Where(n => n.CategoryID == queryParams.CategoryID.Value);
        }

        // FILTER by Status
        if (queryParams.NewsStatus.HasValue)
        {
            query = query.Where(n => n.NewsStatus == queryParams.NewsStatus.Value);
        }

        // FILTER by CreatedBy
        if (queryParams.CreatedByID.HasValue)
        {
            query = query.Where(n => n.CreatedByID == queryParams.CreatedByID.Value);
        }

        // FILTER by Date Range
        if (queryParams.CreatedDateFrom.HasValue)
        {
            query = query.Where(n => n.CreatedDate >= queryParams.CreatedDateFrom.Value);
        }
        if (queryParams.CreatedDateTo.HasValue)
        {
            query = query.Where(n => n.CreatedDate <= queryParams.CreatedDateTo.Value);
        }

        // Count
        var totalItems = await query.CountAsync();

        // SORT
        var sortBy = string.IsNullOrWhiteSpace(queryParams.SortBy) ? "CreatedDate" : queryParams.SortBy;
        query = query.ApplySorting(sortBy, queryParams.SortOrder);

        // PAGING
        query = query.ApplyPaging(queryParams.PageNumber, queryParams.PageSize);

        var articles = await query.ToListAsync();
        var items = articles.Select(n => new NewsArticleResponseDto
        {
            NewsArticleID = n.NewsArticleID,
            NewsTitle = n.NewsTitle,
            Headline = n.Headline,
            CreatedDate = n.CreatedDate,
            NewsSource = n.NewsSource,
            CategoryID = n.CategoryID,
            CategoryName = n.Category?.CategoryName,
            NewsStatus = n.NewsStatus,
            CreatedByID = n.CreatedByID,
            CreatedByName = n.CreatedBy?.AccountName,
            ModifiedDate = n.ModifiedDate
        }).ToList();

        return new PagedResult<NewsArticleResponseDto>
        {
            Items = items,
            PageNumber = queryParams.PageNumber,
            PageSize = queryParams.PageSize,
            TotalItems = totalItems
        };
    }

    /// <summary>
    /// Lấy chi tiết NewsArticle - ĐẦY ĐỦ thông tin: Category, CreatedBy, UpdatedBy, Tags
    /// </summary>
    public async Task<NewsArticleDetailDto?> GetByIdAsync(int id)
    {
        IQueryable<NewsArticle> query = _unitOfWork.NewsArticles.Query(asNoTracking: true);
        
        var article = await query
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.UpdatedBy)
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.NewsArticleID == id);

        if (article == null)
            return null;

        return new NewsArticleDetailDto
        {
            NewsArticleID = article.NewsArticleID,
            NewsTitle = article.NewsTitle,
            Headline = article.Headline,
            CreatedDate = article.CreatedDate,
            NewsContent = article.NewsContent,
            NewsSource = article.NewsSource,
            CategoryID = article.CategoryID,
            NewsStatus = article.NewsStatus,
            CreatedByID = article.CreatedByID,
            UpdatedByID = article.UpdatedByID,
            ModifiedDate = article.ModifiedDate,

            // Category Info
            Category = article.Category == null ? null : new NewsArticleDetailDto.CategoryInfo
            {
                CategoryID = article.Category.CategoryID,
                CategoryName = article.Category.CategoryName
            },

            // CreatedBy Info
            CreatedBy = article.CreatedBy == null ? null : new NewsArticleDetailDto.AccountInfo
            {
                AccountID = article.CreatedBy.AccountID,
                AccountName = article.CreatedBy.AccountName,
                AccountEmail = article.CreatedBy.AccountEmail
            },

            // UpdatedBy Info
            UpdatedBy = article.UpdatedBy == null ? null : new NewsArticleDetailDto.AccountInfo
            {
                AccountID = article.UpdatedBy.AccountID,
                AccountName = article.UpdatedBy.AccountName,
                AccountEmail = article.UpdatedBy.AccountEmail
            },

            // Tags
            Tags = article.Tags.Select(t => new NewsArticleDetailDto.TagInfo
            {
                TagID = t.TagID,
                TagName = t.TagName
            }).ToList()
        };
    }

    public async Task<NewsArticleResponseDto> CreateAsync(NewsArticleCreateDto createDto)
    {
        var article = new NewsArticle
        {
            NewsTitle = createDto.NewsTitle,
            Headline = createDto.Headline,
            NewsContent = createDto.NewsContent,
            NewsSource = createDto.NewsSource,
            CategoryID = createDto.CategoryID,
            NewsStatus = createDto.NewsStatus,
            CreatedByID = createDto.CreatedByID,
            CreatedDate = DateTime.Now
        };

        // Gắn Tags (many-to-many)
        if (createDto.TagIDs.Any())
        {
            var tags = await _unitOfWork.Tags
                .Query(asNoTracking: false)
                .Where(t => createDto.TagIDs.Contains(t.TagID))
                .ToListAsync();
            
            foreach (var tag in tags)
            {
                article.Tags.Add(tag);
            }
        }

        await _unitOfWork.NewsArticles.AddAsync(article);
        await _unitOfWork.SaveChangesAsync();

        return new NewsArticleResponseDto
        {
            NewsArticleID = article.NewsArticleID,
            NewsTitle = article.NewsTitle,
            Headline = article.Headline,
            CreatedDate = article.CreatedDate,
            NewsSource = article.NewsSource,
            CategoryID = article.CategoryID,
            NewsStatus = article.NewsStatus,
            CreatedByID = article.CreatedByID,
            ModifiedDate = article.ModifiedDate
        };
    }

    public async Task<bool> UpdateAsync(NewsArticleUpdateDto updateDto)
    {
        var article = await _unitOfWork.NewsArticles
            .Query(asNoTracking: false)
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.NewsArticleID == updateDto.NewsArticleID);

        if (article == null)
            return false;

        article.NewsTitle = updateDto.NewsTitle;
        article.Headline = updateDto.Headline;
        article.NewsContent = updateDto.NewsContent;
        article.NewsSource = updateDto.NewsSource;
        article.CategoryID = updateDto.CategoryID;
        article.NewsStatus = updateDto.NewsStatus;
        article.UpdatedByID = updateDto.UpdatedByID;
        article.ModifiedDate = DateTime.Now;

        // Update Tags (many-to-many)
        article.Tags.Clear();
        if (updateDto.TagIDs.Any())
        {
            var tags = await _unitOfWork.Tags
                .Query(asNoTracking: false)
                .Where(t => updateDto.TagIDs.Contains(t.TagID))
                .ToListAsync();
            
            foreach (var tag in tags)
            {
                article.Tags.Add(tag);
            }
        }

        _unitOfWork.NewsArticles.Update(article);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var article = await _unitOfWork.NewsArticles.FindByIdAsync(default, id);
        if (article == null)
            return false;

        _unitOfWork.NewsArticles.Remove(article);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
