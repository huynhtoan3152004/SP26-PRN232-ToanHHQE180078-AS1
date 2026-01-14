using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HuynhHuuToan__SE1856_A01_Service.Extensions;

/// <summary>
/// Extension methods để xử lý Search, Sort, Paging cho IQueryable
/// </summary>
public static class QueryExtensions
{
    /// <summary>
    /// Apply pagination (phân trang)
    /// </summary>
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }

    /// <summary>
    /// Apply sorting (sắp xếp) - đơn giản cho người mới
    /// </summary>
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? sortBy, string? sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query;

        // Sử dụng reflection để sort theo property name
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = typeof(T).GetProperty(sortBy);
        
        if (property == null)
            return query;

        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        var methodName = sortOrder?.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
        var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new Type[] { typeof(T), property.PropertyType },
            query.Expression,
            Expression.Quote(orderByExpression));

        return query.Provider.CreateQuery<T>(resultExpression);
    }

    /// <summary>
    /// Parse expand string thành HashSet (để check nhanh)
    /// Ví dụ: "Parent,Children" -> {"parent", "children"}
    /// </summary>
    public static HashSet<string> ParseExpand(string? expand)
    {
        if (string.IsNullOrWhiteSpace(expand))
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        return expand
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }
}
