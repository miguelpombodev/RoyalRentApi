using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Cars;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.Abstractions.Repositories;
using RoyalRent.Application.Cars.Model;
using RoyalRent.Application.Repositories;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Cars.Services;

public class CreateCarService : ICreateCarService
{
    private readonly ICsvProvider _csvProvider;
    private readonly ICarsRepository _carsRepository;
    private readonly IUnitOfWork _unit;

    public CreateCarService(ICsvProvider csvProvider, ICarsRepository carsRepository, IUnitOfWork unit)
    {
        _csvProvider = csvProvider;
        _carsRepository = carsRepository;
        _unit = unit;
    }

    public async Task<Result<string>> InsertCarsDataByCsvFile(IFormFile carsCsvFile)
    {
        var records = _csvProvider.ReadCsvFile<CarsCsv>(carsCsvFile);

        foreach (var data in records)
        {
            var carMakeId = await GetOrCreateEntityIdAsync<CarMake>(
                data.Make,
                () => _carsRepository.CreateOneCarMake(new CarMake(data.Make)));

            var carTypeId = await GetOrCreateEntityIdAsync<CarType>(
                data.Type,
                () => _carsRepository.CreateOneCarType(new CarType(data.Type)));

            var carColorId = await GetOrCreateEntityIdAsync<CarColor>(
                data.Color,
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
        Func<Task<T>> createFunc) where T : BaseEntity
    {
        var entity = await _carsRepository.GetByName<T>(name);
        if (entity is not null)
            return entity.Id;

        var createdEntity = await createFunc();
        return createdEntity.Id;
    }
}
