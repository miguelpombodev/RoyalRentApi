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
            Guid carMakeId;
            Guid carTypeId;
            Guid carColorId;

            var gotCarMake = await _carsRepository.GetByName<CarMake>(data.Make);
            var gotCarType = await _carsRepository.GetByName<CarType>(data.Type);
            var gotCarColor = await _carsRepository.GetByName<CarColor>(data.Color);

            if (gotCarMake is null)
            {
                var carMakeEntity = new CarMake(data.Make);
                var createdCarMake = await _carsRepository.CreateOneCarMake(carMakeEntity);
                carMakeId = createdCarMake.Id;
            }
            else
            {
                carMakeId = gotCarMake.Id;
            }

            if (gotCarType is null)
            {
                var carTypeEntity = new CarType(data.Type);
                var createdCarType = await _carsRepository.CreateOneCarType(carTypeEntity);
                carTypeId = createdCarType.Id;
            }
            else
            {
                carTypeId = gotCarType.Id;
            }

            if (gotCarColor is null)
            {
                var carColorEntity = new CarColor(data.Color);
                var createdCarColor = await _carsRepository.CreateOneCarColor(carColorEntity);
                carColorId = createdCarColor.Id;
            }
            else
            {
                carColorId = gotCarColor.Id;
            }


            var carEntity = new Car(data.Name, data.Model, carMakeId, data.Year, carTypeId,
                carColorId, data.ImageUrl);

            await _carsRepository.CreateOneCar(carEntity);
            await _unit.SaveChangesAsync();
        }


        return Result<string>.Success("success!");
    }
}
