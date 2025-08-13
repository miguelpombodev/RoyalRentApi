using System.Collections.Concurrent;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.Cars.Model;
using RoyalRent.Domain.Abstractions;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Cars.Commands.CreateCarsDataByCsvFile;

public class CreateCarsDataByCsvFileCommandHandler : ICommandHandler<CreateCarsDataByCsvFileCommand, Result<string>>
{
    private readonly ICsvProvider _csvProvider;
    private readonly ICarsRepository _carsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCarsDataByCsvFileCommandHandler(ICsvProvider csvProvider, ICarsRepository carsRepository,
        IUnitOfWork unitOfWork)
    {
        _csvProvider = csvProvider;
        _carsRepository = carsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(CreateCarsDataByCsvFileCommand request,
        CancellationToken cancellationToken)
    {
        ConcurrentDictionary<string, Guid> makeCache = new();
        ConcurrentDictionary<string, Guid> typeCache = new();
        ConcurrentDictionary<string, Guid> colorCache = new();
        ConcurrentDictionary<string, Guid> transmissionsCache = new();
        ConcurrentDictionary<string, Guid> fuelTypeCache = new();

        var records = _csvProvider.ReadCsvFile<CarsCsv>(request.carsCsvFile);

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

            var carTransmissionId = await GetOrCreateEntityIdAsync<CarTransmissions>(
                data.Color,
                transmissionsCache,
                () => _carsRepository.CreateOneCarTransmission(new CarTransmissions(data.Color)));

            var carFuelTypeId = await GetOrCreateEntityIdAsync<CarFuelType>(
                data.Color,
                fuelTypeCache,
                () => _carsRepository.CreateOneCarFuelType(new CarFuelType(data.Color)));


            var carEntity = new Car(data.Name, data.Model, carMakeId, data.Year, carTypeId,
                carColorId, data.ImageUrl, carTransmissionId, carFuelTypeId, data.Seats, data.Price, data.Description);

            await _carsRepository.CreateOneCar(carEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
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
