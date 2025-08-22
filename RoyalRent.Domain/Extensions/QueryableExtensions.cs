using System.Linq.Expressions;

namespace RoyalRent.Domain.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }

    public static IQueryable<T> WhereIfAny<T, TCollection>(this IQueryable<T> source,
        IEnumerable<TCollection>? collection, Expression<Func<T, bool>> predicate)
    {
        return collection?.Any() == true ? source.Where(predicate) : source;
    }
}
