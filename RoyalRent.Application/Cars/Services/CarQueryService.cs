using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Cars;
using RoyalRent.Application.Abstractions.Repositories;
using RoyalRent.Application.Cars.DTOs;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Cars.Services;

public class CarQueryService : ICarQueryService
{
    private readonly ICarsRepository _carsRepository;

    public CarQueryService(ICarsRepository carsRepository)
    {
        _carsRepository = carsRepository;
    }

    public Task<Car> GetCarByName(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<AvailableCars>>> GetAvailableCarsAsync()
    {
        var getAvailableCars = await _carsRepository.GetAvailableCarsAsync();

        var availableCars = getAvailableCars
            .Select(available => new AvailableCars(available.Name, available.Category,
                available.Price, available.Seats, available.ImageUrl, available.Transmission, available.FuelType,
                available.Description))
            .ToList();
        return Result<List<AvailableCars>>.Success(availableCars);
    }
}
