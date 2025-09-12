namespace RoyalRent.Domain.Abstractions.Filters;

public interface IGetAllAvailableCarsFilters
{
    bool IsFeatured { get; }
    List<string> CarTypeNames { get; set; }
    List<string> CarColorNames { get; set; }
    List<string> CarFuelTypeNames { get; set; }
    List<string> CarTransmissionsNames { get; set; }
}
