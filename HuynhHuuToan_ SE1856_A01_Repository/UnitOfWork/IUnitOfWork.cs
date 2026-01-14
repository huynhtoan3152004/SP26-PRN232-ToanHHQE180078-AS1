using System;
using System.Threading;
using System.Threading.Tasks;
using HuynhHuuToan__SE1856_A01_Repository.Models.Entities;
using HuynhHuuToan__SE1856_A01_Repository.Repositories;

namespace HuynhHuuToan__SE1856_A01_Repository.UnitOfWork;

/// <summary>
/// Unit of Work Interface - quản lý tất cả repositories và transactions
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Repository cho Category
    /// </summary>
    IGenericRepository<Category> Categories { get; }

    /// <summary>
    /// Repository cho NewsArticle
    /// </summary>
    IGenericRepository<NewsArticle> NewsArticles { get; }

    /// <summary>
    /// Repository cho SystemAccount
    /// </summary>
    IGenericRepository<SystemAccount> SystemAccounts { get; }

    /// <summary>
    /// Repository cho Tag
    /// </summary>
    IGenericRepository<Tag> Tags { get; }

    /// <summary>
    /// Lưu tất cả thay đổi vào database (transaction)
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
