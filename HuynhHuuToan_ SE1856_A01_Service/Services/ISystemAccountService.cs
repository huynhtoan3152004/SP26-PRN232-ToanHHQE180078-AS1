using HuynhHuuToan__SE1856_A01_Service.Common;
using HuynhHuuToan__SE1856_A01_Service.DTOs.SystemAccount;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;

namespace HuynhHuuToan__SE1856_A01_Service.Services;

public interface ISystemAccountService
{
    Task<PagedResult<SystemAccountResponseDto>> GetAllAsync(SystemAccountQueryParams queryParams);
    Task<SystemAccountDetailDto?> GetByIdAsync(int id);
    Task<SystemAccountResponseDto> CreateAsync(SystemAccountCreateDto createDto);
    Task<bool> UpdateAsync(SystemAccountUpdateDto updateDto);
    Task<bool> DeleteAsync(int id);
}
