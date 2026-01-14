using HuynhHuuToan__SE1856_A01_Service.DTOs.SystemAccount;
using HuynhHuuToan__SE1856_A01_Service.QueryParams;
using HuynhHuuToan__SE1856_A01_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace HuynhHuuToan__SE1856_A01_BE.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SystemAccountController : ControllerBase
{
    private readonly ISystemAccountService _accountService;

    public SystemAccountController(ISystemAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts([FromQuery] SystemAccountQueryParams queryParams)
    {
        var result = await _accountService.GetAllAsync(queryParams);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccount(int id)
    {
        var account = await _accountService.GetByIdAsync(id);
        if (account == null)
            return NotFound(new { message = $"Account with ID {id} not found" });

        return Ok(account);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] SystemAccountCreateDto createDto)
    {
        var result = await _accountService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetAccount), new { id = result.AccountID }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(int id, [FromBody] SystemAccountUpdateDto updateDto)
    {
        if (id != updateDto.AccountID)
            return BadRequest(new { message = "ID mismatch" });

        var result = await _accountService.UpdateAsync(updateDto);
        if (!result)
            return NotFound(new { message = $"Account with ID {id} not found" });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var result = await _accountService.DeleteAsync(id);
        if (!result)
            return NotFound(new { message = $"Account with ID {id} not found" });

        return NoContent();
    }
}
