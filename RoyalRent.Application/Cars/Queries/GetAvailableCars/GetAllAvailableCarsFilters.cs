using RoyalRent.Domain.Abstractions.Filters;

namespace RoyalRent.Application.Cars.Queries.GetAvailableCars;

public record GetAllAvailableCarsFilters : IGetAllAvailableCarsFilters
{
    public bool IsFeatured { get; set; } = false;
    public List<string> CarTypeNames { get; set; } = [];
    public List<string> CarColorNames { get; set; } = [];
    public List<string> CarFuelTypeNames { get; set; } = [];
    public List<string> CarTransmissionsNames { get; set; } = [];
}

public record CarSortRequest : ICarSortRequest
{
    public CarSortBy SortBy { get; set; } = CarSortBy.Name;
    public SortType SortType { get; set; } = SortType.Asc;
}
