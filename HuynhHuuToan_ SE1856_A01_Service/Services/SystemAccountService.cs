using HuynhHuuToan__SE1856_A01_Repository.Models.Entities;
using HuynhHuuToan__SE1856_A01_Repository.UnitOfWork;
using HuynhHuuToan__SE1856_A01_Service.Common;
using HuynhHuuToan__SE1856_A01_Service.DTOs.SystemAccount;
using HuynhHuuToan__SE1856_A01_Service.Extensions;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;
using Microsoft.EntityFrameworkCore;

namespace HuynhHuuToan__SE1856_A01_Service.Services;

public class SystemAccountService : ISystemAccountService
{
    private readonly IUnitOfWork _unitOfWork;

    public SystemAccountService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<SystemAccountResponseDto>> GetAllAsync(SystemAccountQueryParams queryParams)
    {
        var query = _unitOfWork.SystemAccounts.Query(asNoTracking: true);

        // SEARCH
        if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
        {
            var searchLower = queryParams.SearchTerm.ToLower();
            query = query.Where(a =>
                a.AccountName.ToLower().Contains(searchLower) ||
                a.AccountEmail.ToLower().Contains(searchLower)
            );
        }

        // FILTER by Role
        if (queryParams.AccountRole.HasValue)
        {
            query = query.Where(a => a.AccountRole == queryParams.AccountRole.Value);
        }

        // Count
        var totalItems = await query.CountAsync();

        // SORT
        var sortBy = string.IsNullOrWhiteSpace(queryParams.SortBy) ? "AccountName" : queryParams.SortBy;
        query = query.ApplySorting(sortBy, queryParams.SortOrder);

        // PAGING
        query = query.ApplyPaging(queryParams.PageNumber, queryParams.PageSize);

        var accounts = await query.ToListAsync();
        var items = accounts.Select(a => new SystemAccountResponseDto
        {
            AccountID = a.AccountID,
            AccountName = a.AccountName,
            AccountEmail = a.AccountEmail,
            AccountRole = a.AccountRole
        }).ToList();

        return new PagedResult<SystemAccountResponseDto>
        {
            Items = items,
            Page = queryParams.PageNumber,
            PageSize = queryParams.PageSize,
            TotalItems = totalItems
        };
    }

    public async Task<SystemAccountDetailDto?> GetByIdAsync(int id)
    {
        var account = await _unitOfWork.SystemAccounts
            .Query(asNoTracking: true)
            .Include(a => a.NewsArticleCreatedBies)
            .Include(a => a.NewsArticleUpdatedBies)
            .FirstOrDefaultAsync(a => a.AccountID == id);

        if (account == null)
            return null;

        return new SystemAccountDetailDto
        {
            AccountID = account.AccountID,
            AccountName = account.AccountName,
            AccountEmail = account.AccountEmail,
            AccountRole = account.AccountRole,
            CreatedNewsCount = account.NewsArticleCreatedBies.Count,
            UpdatedNewsCount = account.NewsArticleUpdatedBies.Count
        };
    }

    public async Task<SystemAccountResponseDto> CreateAsync(SystemAccountCreateDto createDto)
    {
        var account = new SystemAccount
        {
            AccountName = createDto.AccountName,
            AccountEmail = createDto.AccountEmail,
            AccountRole = createDto.AccountRole,
            AccountPassword = createDto.AccountPassword // Note: Should hash password in production
        };

        await _unitOfWork.SystemAccounts.AddAsync(account);
        await _unitOfWork.SaveChangesAsync();

        return new SystemAccountResponseDto
        {
            AccountID = account.AccountID,
            AccountName = account.AccountName,
            AccountEmail = account.AccountEmail,
            AccountRole = account.AccountRole
        };
    }

    public async Task<bool> UpdateAsync(SystemAccountUpdateDto updateDto)
    {
        var account = await _unitOfWork.SystemAccounts.FindByIdAsync(default, updateDto.AccountID);
        if (account == null)
            return false;

        account.AccountName = updateDto.AccountName;
        account.AccountEmail = updateDto.AccountEmail;
        account.AccountRole = updateDto.AccountRole;
        account.AccountPassword = updateDto.AccountPassword; // Note: Should hash password

        _unitOfWork.SystemAccounts.Update(account);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var account = await _unitOfWork.SystemAccounts.FindByIdAsync(default, id);
        if (account == null)
            return false;

        _unitOfWork.SystemAccounts.Remove(account);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
