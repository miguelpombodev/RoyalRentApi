namespace RoyalRent.Application.Cars.Queries.GetAvailableCars;

public sealed record GetAvailableCarsResponse(
    string name,
    string category,
    decimal price,
    int seats,
    string imageUrl,
    string transmission,
    string fuelType,
    string description);
