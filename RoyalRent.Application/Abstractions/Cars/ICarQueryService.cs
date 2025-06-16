using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Abstractions.Cars;

public interface ICarQueryService
{
    Task<Car> GetCarByName(string name);
}
