using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HuynhHuuToan__SE1856_A01_Repository.Repositories;

/// <summary>
/// Generic Repository Interface - định nghĩa các method chung cho tất cả entities
/// </summary>
public interface IGenericRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Lấy IQueryable để có thể query linh hoạt
    /// </summary>
    IQueryable<TEntity> Query(bool asNoTracking = true);

    /// <summary>
    /// Tìm entity theo ID (Primary Key)
    /// </summary>
    ValueTask<TEntity?> FindByIdAsync(CancellationToken cancellationToken = default, params object[] keyValues);

    /// <summary>
    /// Thêm entity mới
    /// </summary>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật entity
    /// </summary>
    void Update(TEntity entity);

    /// <summary>
    /// Xóa entity
    /// </summary>
    void Remove(TEntity entity);
}
