using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Abstractions.Repositories;

public interface ICarsRepository
{
    Task<Car> CreateOneCar(Car car);
    Task<CarMake> CreateOneCarMake(CarMake carMake);
    Task<CarType> CreateOneCarType(CarType carType);
    Task<CarColor> CreateOneCarColor(CarColor carColor);
    Task<T?> GetByName<T>(string name) where T : class;
}
