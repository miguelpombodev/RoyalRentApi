namespace RoyalRent.Application.Cars.Queries.GetAvailableCarsFiltersValues;

public record GetAvailableCarsFiltersValuesQueryResponse(
    List<string> carTypesNames,
    List<string> carFuelTypesNames,
    List<string> carColorsNames,
    List<string> carTransmissionsNames
);
