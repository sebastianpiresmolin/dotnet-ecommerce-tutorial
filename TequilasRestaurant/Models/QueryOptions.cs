using System.Linq.Expressions;

namespace TequilasRestaurant.Models;

public class QueryOptions<T> where T : class
{
    private string[] includes = Array.Empty<string>();

    public Expression<Func<T, object>> OrderBy { get; set; } = null!;

    public Expression<Func<T, bool>> Where { get; set; } = null!;

    public string Includes
    {
        set => GetIncludes = value.Replace(" ", "").Split(",");
    }

    public string[] GetIncludes { get; private set; } = Array.Empty<string>();

    public bool HasWhere => Where != null;

    public bool HasOrderBy => OrderBy != null;
}