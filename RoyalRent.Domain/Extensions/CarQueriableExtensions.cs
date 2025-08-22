using System.Linq.Expressions;
using RoyalRent.Domain.Abstractions.Filters;
using RoyalRent.Domain.Constants;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Domain.Extensions;

public static class CarQueriableExtensions
{
    private static readonly Dictionary<string, Expression<Func<Car, object>>> SortExpressions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [CarSortFields.Name] = c => c.Name,
            [CarSortFields.Model] = c => c.Model,
            [CarSortFields.Year] = c => c.Year,
            [CarSortFields.Price] = c => c.Price,
            [CarSortFields.Make] = c => c.CarMake.Name,
            [CarSortFields.Type] = c => c.CarType.Name,
            [CarSortFields.Seats] = c => c.Seats
        };

    public static IQueryable<Car> ApplySorting(this IQueryable<Car> source, ICarSortRequest sortRequest)
    {
        var sortBy = sortRequest.SortBy;
        var sortDirection = sortRequest.SortType;

        if (!SortExpressions.TryGetValue(sortBy.ToString(), out var expression))
        {
            expression = SortExpressions[CarSortFields.Name];
        }

        var isDescending = string.Equals(sortDirection.ToString(), SortDirections.Desc, StringComparison.OrdinalIgnoreCase);

        return isDescending ? source.OrderByDescending(expression) : source.OrderBy(expression);
    }
}
