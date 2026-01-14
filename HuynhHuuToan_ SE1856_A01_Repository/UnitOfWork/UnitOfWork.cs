using System;
using System.Threading;
using System.Threading.Tasks;
using HuynhHuuToan__SE1856_A01_Repository.Models.Data;
using HuynhHuuToan__SE1856_A01_Repository.Models.Entities;
using HuynhHuuToan__SE1856_A01_Repository.Repositories;

namespace HuynhHuuToan__SE1856_A01_Repository.UnitOfWork;

/// <summary>
/// Unit of Work Implementation - quản lý repositories và SaveChanges
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly FUNewsManagementSystemContext _context;

    // Lazy initialization cho các repositories
    private IGenericRepository<Category>? _categories;
    private IGenericRepository<NewsArticle>? _newsArticles;
    private IGenericRepository<SystemAccount>? _systemAccounts;
    private IGenericRepository<Tag>? _tags;

    public UnitOfWork(FUNewsManagementSystemContext context)
    {
        _context = context;
    }

    public IGenericRepository<Category> Categories 
        => _categories ??= new GenericRepository<Category>(_context);

    public IGenericRepository<NewsArticle> NewsArticles 
        => _newsArticles ??= new GenericRepository<NewsArticle>(_context);

    public IGenericRepository<SystemAccount> SystemAccounts 
        => _systemAccounts ??= new GenericRepository<SystemAccount>(_context);

    public IGenericRepository<Tag> Tags 
        => _tags ??= new GenericRepository<Tag>(_context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
