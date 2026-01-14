using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HuynhHuuToan__SE1856_A01_Repository.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace HuynhHuuToan__SE1856_A01_Repository.Repositories;

/// <summary>
/// Generic Repository Implementation - triển khai các method chung
/// </summary>
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly FUNewsManagementSystemContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(FUNewsManagementSystemContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public IQueryable<TEntity> Query(bool asNoTracking = true)
    {
        return asNoTracking ? _dbSet.AsNoTracking() : _dbSet;
    }

    public ValueTask<TEntity?> FindByIdAsync(CancellationToken cancellationToken = default, params object[] keyValues)
    {
        return _dbSet.FindAsync(keyValues, cancellationToken);
    }

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return _dbSet.AddAsync(entity, cancellationToken).AsTask();
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}
