using HuynhHuuToan__SE1856_A01_BE.Models;
using HuynhHuuToan__SE1856_A01_Service.Common;
using HuynhHuuToan__SE1856_A01_Service.DTOs.SystemAccount;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;
using HuynhHuuToan__SE1856_A01_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HuynhHuuToan__SE1856_A01_BE.Controllers;

/// <summary>
/// API Controller cho SystemAccount - CRUD operations
/// Route sử dụng danh từ số nhiều theo RESTful convention
/// </summary>
[Route("api/accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly ISystemAccountService _accountService;

    public AccountsController(ISystemAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// GET: api/accounts
    /// Lấy danh sách Account - Yêu cầu đăng nhập
    /// </summary>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<SystemAccountResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccounts([FromQuery] SystemAccountQueryParams queryParams)
    {
        var result = await _accountService.GetAllAsync(queryParams);
        return Ok(ApiResponse<PagedResult<SystemAccountResponseDto>>.SuccessResponse(result, "Accounts retrieved successfully"));
    }

    /// <summary>
    /// GET: api/accounts/{id}
    /// Lấy chi tiết Account - Yêu cầu đăng nhập
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<SystemAccountDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccount(int id)
    {
        var account = await _accountService.GetByIdAsync(id);
        if (account == null)
            return NotFound(ApiResponse<object>.NotFoundResponse($"Account with ID {id} not found"));

        return Ok(ApiResponse<SystemAccountDetailDto>.SuccessResponse(account, "Account retrieved successfully"));
    }

    /// <summary>
    /// POST: api/accounts
    /// Tạo mới Account - Yêu cầu đăng nhập (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<SystemAccountResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateAccount([FromBody] SystemAccountCreateDto createDto)
    {
        var result = await _accountService.CreateAsync(createDto);
        var response = ApiResponse<SystemAccountResponseDto>.SuccessResponse(result, "Account created successfully");
        return CreatedAtAction(nameof(GetAccount), new { id = result.AccountID }, response);
    }

    /// <summary>
    /// PUT: api/accounts/{id}
    /// Cập nhật Account - Yêu cầu đăng nhập
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAccount(int id, [FromBody] SystemAccountUpdateDto updateDto)
    {
        if (id != updateDto.AccountID)
            return BadRequest(ApiResponse<object>.FailResponse("ID mismatch", new List<string> { $"Route ID: {id}, Body ID: {updateDto.AccountID}" }));

        var result = await _accountService.UpdateAsync(updateDto);
        if (!result)
            return NotFound(ApiResponse<object>.NotFoundResponse($"Account with ID {id} not found"));

        return Ok(ApiResponse.SuccessResponse("Account updated successfully"));
    }

    /// <summary>
    /// DELETE: api/accounts/{id}
    /// Xóa Account - Yêu cầu đăng nhập (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var result = await _accountService.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<object>.NotFoundResponse($"Account with ID {id} not found"));

        return Ok(ApiResponse.SuccessResponse("Account deleted successfully"));
    }
}
