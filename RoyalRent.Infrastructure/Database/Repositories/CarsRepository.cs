using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoyalRent.Application.Cars.Queries.GetAvailableCars;
using RoyalRent.Domain.Abstractions;
using RoyalRent.Domain.Abstractions.Entities;
using RoyalRent.Domain.Abstractions.Filters;
using RoyalRent.Domain.Data.Models;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Extensions;

namespace RoyalRent.Infrastructure.Database.Repositories;

public class CarsRepository : ICarsRepository
{
    public CarsRepository(ApiDbContext context, IDbContextFactory<ApiDbContext> factory, ILogger<CarsRepository> logger)
    {
        _factory = factory;
        _context = context;
        _carContext = context.Set<Car>();
        _carMakeContext = context.Set<CarMake>();
        _carTypeContext = context.Set<CarType>();
        _carColorContext = context.Set<CarColor>();
        _carTransmissionContext = context.Set<CarTransmissions>();
        _carFuelTypeContext = context.Set<CarFuelType>();
        _logger = logger;
    }

    private readonly ILogger _logger;

    private readonly ApiDbContext _context;
    private readonly IDbContextFactory<ApiDbContext> _factory;
    private readonly DbSet<Car> _carContext;
    private readonly DbSet<CarMake> _carMakeContext;
    private readonly DbSet<CarType> _carTypeContext;
    private readonly DbSet<CarColor> _carColorContext;
    private readonly DbSet<CarTransmissions> _carTransmissionContext;
    private readonly DbSet<CarFuelType> _carFuelTypeContext;

    public async Task<Car> CreateOneCar(Car car)
    {
        var carEntry = await _carContext.AddAsync(car);

        _logger.LogInformation("A car {CarName} was created successfully at {CarCreatedAt}", car.Name, car.CreatedOn);

        return carEntry.Entity;
    }

    public async Task<CarMake> CreateOneCarMake(CarMake carMake)
    {
        var carMakeEntry = await _carMakeContext.AddAsync(carMake);

        return carMakeEntry.Entity;
    }

    public async Task<CarType> CreateOneCarType(CarType carType)
    {
        var carTypeEntry = await _carTypeContext.AddAsync(carType);

        return carTypeEntry.Entity;
    }

    public async Task<CarColor> CreateOneCarColor(CarColor carColor)
    {
        var carColorEntry = await _carColorContext.AddAsync(carColor);

        return carColorEntry.Entity;
    }

    public async Task<CarTransmissions> CreateOneCarTransmission(CarTransmissions carTransmissions)
    {
        var carTransmissionEntry = await _carTransmissionContext.AddAsync(carTransmissions);

        return carTransmissionEntry.Entity;
    }

    public async Task<CarFuelType> CreateOneCarFuelType(CarFuelType carFuelType)
    {
        var carFuelTypeEntry = await _carFuelTypeContext.AddAsync(carFuelType);

        return carFuelTypeEntry.Entity;
    }

    public async Task<List<GetAvailableCars>> GetAvailableCarsAsync(IGetAllAvailableCarsFilters filters,
        ICarSortRequest sort)
    {
        var availableCarsQuery = _carContext
            .Include(car => car.CarMake)
            .Include(car => car.CarColor)
            .Include(car => car.CarType)
            .Include(car => car.CarFuelType)
            .Include(car => car.CarTransmissions)
            .AsNoTracking()
            .WhereIf(filters.IsFeatured, car => car.IsFeatured.Equals(filters.IsFeatured))
            .WhereIfAny(filters.CarColorNames, car => filters.CarColorNames.Contains(car.CarColor.Name))
            .WhereIfAny(filters.CarFuelTypeNames, car => filters.CarFuelTypeNames.Contains(car.CarFuelType.Name))
            .WhereIfAny(filters.CarTransmissionsNames,
                car => filters.CarTransmissionsNames.Contains(car.CarTransmissions.Name))
            .WhereIfAny(filters.CarTypeNames, car => filters.CarTypeNames.Contains(car.CarType.Name))
            .ApplySorting(sort);

        var availableCars = await availableCarsQuery.Select(car =>
                new GetAvailableCars(
                    car.Name,
                    car.CarType!.Name,
                    car.Price, car.Seats,
                    car.ImageUrl,
                    car.CarTransmissions!.Name,
                    car.CarFuelType!.Name,
                    car.Description,
                    car.IsFeatured))
            .ToListAsync();

        return availableCars;
    }

    public async Task<List<string>> GetFilterValues<T>() where T : class, ICarBaseEntity
    {
        await using var context = await _factory.CreateDbContextAsync();
        return await context.Set<T>().AsNoTracking().Select(t => t.Name).Distinct().ToListAsync();
    }

    public async Task<T?> GetByName<T>(string name) where T : class
    {
        var item = await _context.Set<T>().AsNoTracking()
            .FirstOrDefaultAsync(p => EF.Property<string>(p, "Name") == name);

        return item;
    }
}
