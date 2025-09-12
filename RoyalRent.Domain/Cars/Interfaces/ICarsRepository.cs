using RoyalRent.Domain.Abstractions.Entities;
using RoyalRent.Domain.Abstractions.Filters;
using RoyalRent.Domain.Cars.Entities;
using RoyalRent.Domain.Data.Models;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Domain.Cars.Interfaces;

public interface ICarsRepository
{
    Task<Car> CreateOneCar(Car car);
    Task<Car?> GetCarById(Guid carId);
    Task<CarMake> CreateOneCarMake(CarMake carMake);
    Task<CarType> CreateOneCarType(CarType carType);
    Task<CarColor> CreateOneCarColor(CarColor carColor);
    Task<CarFuelType> CreateOneCarFuelType(CarFuelType carFuelType);
    Task<CarTransmissions> CreateOneCarTransmission(CarTransmissions carTransmissions);
    Task<List<GetAvailableCars>> GetAvailableCarsAsync(IGetAllAvailableCarsFilters filters, ICarSortRequest sort);
    Task<List<string>> GetFilterValues<T>() where T : class, ICarBaseEntity;
    Task<T?> GetByName<T>(string name) where T : class;
}
