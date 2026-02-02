using System.Dynamic;
using System.Reflection;

namespace HuynhHuuToan__SE1856_A01_Service.Common;

/// <summary>
/// Helper để tạo dynamic object chỉ chứa các fields được chọn
/// Giúp giảm payload response khi client chỉ cần một số fields
/// </summary>
public static class DynamicResponseHelper
{
    /// <summary>
    /// Lọc object chỉ giữ lại các fields được chỉ định
    /// </summary>
    /// <typeparam name="T">Type của entity</typeparam>
    /// <param name="entity">Entity cần shape</param>
    /// <param name="fields">Danh sách fields cần giữ lại</param>
    /// <returns>ExpandoObject chỉ chứa các fields được chọn</returns>
    public static ExpandoObject ShapeData<T>(T entity, List<string> fields)
    {
        var expandoObject = new ExpandoObject();
        var expandoDict = (IDictionary<string, object?>)expandoObject;

        if (fields == null || !fields.Any())
        {
            // Nếu không chỉ định fields, trả về tất cả properties
            var allProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in allProperties)
            {
                var value = prop.GetValue(entity);
                expandoDict.Add(prop.Name, value);
            }
        }
        else
        {
            // Chỉ trả về các fields được chỉ định
            foreach (var field in fields)
            {
                var prop = typeof(T).GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop != null)
                {
                    var value = prop.GetValue(entity);
                    expandoDict.Add(prop.Name, value);
                }
            }
        }

        return expandoObject;
    }

    /// <summary>
    /// Lọc danh sách objects chỉ giữ lại các fields được chỉ định
    /// </summary>
    /// <typeparam name="T">Type của entity</typeparam>
    /// <param name="entities">Danh sách entities</param>
    /// <param name="fields">Danh sách fields cần giữ lại</param>
    /// <returns>Danh sách ExpandoObjects</returns>
    public static List<ExpandoObject> ShapeData<T>(IEnumerable<T> entities, List<string> fields)
    {
        return entities.Select(e => ShapeData(e, fields)).ToList();
    }
}
