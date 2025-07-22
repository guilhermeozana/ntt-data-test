using System.Linq.Expressions;

namespace SalesRecords.Shared.SharedKernel.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
            return query;

        var clauses = order.Split(',');

        bool isFirst = true;

        foreach (var clause in clauses)
        {
            var tokens = clause.Trim().Split(' ');
            var propertyName = tokens[0];
            var descending = tokens.Length > 1 && tokens[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

            query = ApplyOrder(query, propertyName, descending, isFirst);
            isFirst = false;
        }

        return query;
    }

    private static IQueryable<T> ApplyOrder<T>(IQueryable<T> query, string propertyName, bool descending, bool isFirst)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.PropertyOrField(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);

        var methodName = isFirst
            ? (descending ? "OrderByDescending" : "OrderBy")
            : (descending ? "ThenByDescending" : "ThenBy");

        var method = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type);

        return (IQueryable<T>)method.Invoke(null, new object[] { query, lambda })!;
    }

    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int page, int size)
    {
        var skip = (page - 1) * size;
        var take = size;

        return query.Skip(skip).Take(take);
    }
    
    public static IQueryable<T> ApplyFiltering<T>(
    this IQueryable<T> query,
    Dictionary<string, string>? filters,
    Dictionary<string, decimal>? minValues,
    Dictionary<string, decimal>? maxValues)
{
    var parameter = Expression.Parameter(typeof(T), "x");

    if (filters is not null)
    {
        foreach (var (key, value) in filters)
        {
            if (string.IsNullOrWhiteSpace(value)) continue;

            var property = Expression.PropertyOrField(parameter, key);
            var constant = Expression.Constant(value);
            Expression comparison;

            if (value.StartsWith("*") && value.EndsWith("*"))
            {
                comparison = Expression.Call(property, "Contains", null, Expression.Constant(value.Trim('*')));
            }
            else if (value.StartsWith("*"))
            {
                comparison = Expression.Call(property, "EndsWith", null, Expression.Constant(value.TrimStart('*')));
            }
            else if (value.EndsWith("*"))
            {
                comparison = Expression.Call(property, "StartsWith", null, Expression.Constant(value.TrimEnd('*')));
            }
            else
            {
                comparison = Expression.Equal(property, constant);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
            query = query.Where(lambda);
        }
    }

    if (minValues is not null)
    {
        foreach (var (key, min) in minValues)
        {
            var property = Expression.PropertyOrField(parameter, key);
            var constant = Expression.Constant(min);
            var comparison = Expression.GreaterThanOrEqual(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
            query = query.Where(lambda);
        }
    }

    if (maxValues is not null)
    {
        foreach (var (key, max) in maxValues)
        {
            var property = Expression.PropertyOrField(parameter, key);
            var constant = Expression.Constant(max);
            var comparison = Expression.LessThanOrEqual(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
            query = query.Where(lambda);
        }
    }

    return query;
}
}