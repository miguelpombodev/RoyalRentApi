using RoyalRent.Application.Abstractions.Cars;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Cars.Services;

public class CarQueryService : ICarQueryService
{
    public Task<Car> GetCarByName(string name)
    {
        throw new NotImplementedException();
    }
}
