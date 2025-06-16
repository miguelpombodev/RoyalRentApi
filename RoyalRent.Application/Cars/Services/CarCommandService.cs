using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Cars;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.Abstractions.Repositories;
using RoyalRent.Application.Cars.Model;
using RoyalRent.Application.Repositories;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Cars.Services;

public class CarCommandService : ICarCommandService
{
    private readonly ICsvProvider _csvProvider;
    private readonly ICarsRepository _carsRepository;
    private readonly IUnitOfWork _unit;

    public CarCommandService(ICsvProvider csvProvider, ICarsRepository carsRepository, IUnitOfWork unit)
    {
        _csvProvider = csvProvider;
        _carsRepository = carsRepository;
        _unit = unit;
    }

    public async Task<Result<string>> InsertCarsDataByCsvFile(IFormFile carsCsvFile)
    {
        ConcurrentDictionary<string, Guid> makeCache = new();
        ConcurrentDictionary<string, Guid> typeCache = new();
        ConcurrentDictionary<string, Guid> colorCache = new();

        var records = _csvProvider.ReadCsvFile<CarsCsv>(carsCsvFile);

        foreach (var data in records)
        {
            var carMakeId = await GetOrCreateEntityIdAsync<CarMake>(
                data.Make,
                makeCache,
                () => _carsRepository.CreateOneCarMake(new CarMake(data.Make)));

            var carTypeId = await GetOrCreateEntityIdAsync<CarType>(
                data.Type,
                typeCache,
                () => _carsRepository.CreateOneCarType(new CarType(data.Type)));

            var carColorId = await GetOrCreateEntityIdAsync<CarColor>(
                data.Color,
                colorCache,
                () => _carsRepository.CreateOneCarColor(new CarColor(data.Color)));


            var carEntity = new Car(data.Name, data.Model, carMakeId, data.Year, carTypeId,
                carColorId, data.ImageUrl);

            await _carsRepository.CreateOneCar(carEntity);
            await _unit.SaveChangesAsync();
        }


        return Result<string>.Success("success!");
    }

    private async Task<Guid> GetOrCreateEntityIdAsync<T>(
        string name,
        ConcurrentDictionary<string, Guid> cache,
        Func<Task<T>> createFunc) where T : BaseEntity
    {
        if (cache.TryGetValue(name, out var cacheId))
            return cacheId;

        var entity = await _carsRepository.GetByName<T>(name);
        if (entity is not null)
            return entity.Id;

        var createdEntity = await createFunc();
        cache.TryAdd(name, createdEntity.Id);
        return createdEntity.Id;
    }
}
