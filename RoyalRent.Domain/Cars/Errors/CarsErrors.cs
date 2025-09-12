using RoyalRent.Domain.Errors;

namespace RoyalRent.Domain.Cars.Errors;

public static class CarsErrors
{
    public static readonly Error CarNotFound = new("Car.CarNotFound", 404, "Car not found");
}
