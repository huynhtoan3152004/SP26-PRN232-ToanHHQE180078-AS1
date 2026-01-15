using Microsoft.AspNetCore.Mvc;
using HuynhHuuToan__SE1856_A01_Repository.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace HuynhHuuToan__SE1856_A01_BE.Controllers;

/// <summary>
/// Health check endpoint to verify database connection and data
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    private readonly FUNewsManagementSystemContext _context;

    public HealthController(FUNewsManagementSystemContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET: api/Health - Check API and database health
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            // Test database connection
            var canConnect = await _context.Database.CanConnectAsync();
            if (!canConnect)
            {
                return StatusCode(500, new
                {
                    status = "unhealthy",
                    database = "cannot connect",
                    error = "Unable to connect to database"
                });
            }

            // Get record counts
            var categoryCount = await _context.Categories.CountAsync();
            var tagCount = await _context.Tags.CountAsync();
            var accountCount = await _context.SystemAccounts.CountAsync();
            var newsCount = await _context.NewsArticles.CountAsync();

            return Ok(new
            {
                status = "healthy",
                database = "connected",
                data = new
                {
                    categories = categoryCount,
                    tags = tagCount,
                    accounts = accountCount,
                    newsArticles = newsCount
                },
                timestamp = DateTime.UtcNow,
                message = categoryCount > 0 ? "Database has data" : "Database is empty - run seed script"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "unhealthy",
                database = "error",
                error = ex.Message,
                innerError = ex.InnerException?.Message
            });
        }
    }

    /// <summary>
    /// GET: api/Health/test - Quick connection test
    /// </summary>
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new
        {
            status = "API is running",
            timestamp = DateTime.UtcNow,
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
        });
    }
}
