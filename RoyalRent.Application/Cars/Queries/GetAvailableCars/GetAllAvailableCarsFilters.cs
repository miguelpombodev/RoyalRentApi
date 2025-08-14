using RoyalRent.Domain.Abstractions.Filters;

namespace RoyalRent.Application.Cars.Queries.GetAvailableCars;

public record GetAllAvailableCarsFilters : IGetAllAvailableCarsFilters
{
    public bool IsFeatured { get; set; } = false;
}
