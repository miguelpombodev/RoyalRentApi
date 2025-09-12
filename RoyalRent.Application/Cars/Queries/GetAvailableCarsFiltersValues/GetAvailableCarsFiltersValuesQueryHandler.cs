using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Domain.Abstractions;
using RoyalRent.Domain.Cars.Interfaces;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Cars.Queries.GetAvailableCarsFiltersValues;

public class GetAvailableCarsFiltersValuesQueryHandler : IQueryHandler<GetAvailableCarsFiltersValuesQuery,
    Result<GetAvailableCarsFiltersValuesQueryResponse>>
{
    private readonly ICarsRepository _carsRepository;

    public GetAvailableCarsFiltersValuesQueryHandler(ICarsRepository carsRepository)
    {
        _carsRepository = carsRepository;
    }

    public async Task<Result<GetAvailableCarsFiltersValuesQueryResponse>> Handle(
        GetAvailableCarsFiltersValuesQuery request,
        CancellationToken cancellationToken)
    {
        var colorsTask = _carsRepository.GetFilterValues<CarColor>();
        var fuelTypesTask = _carsRepository.GetFilterValues<CarFuelType>();
        var transmissionsTask = _carsRepository.GetFilterValues<CarTransmissions>();
        var typesTask = _carsRepository.GetFilterValues<CarType>();

        await Task.WhenAll(
            colorsTask, fuelTypesTask, transmissionsTask, typesTask
        );

        var colors = await colorsTask;
        var fuelTypes = await fuelTypesTask;
        var transmissions = await transmissionsTask;
        var types = await typesTask;

        var response = new GetAvailableCarsFiltersValuesQueryResponse(
            types,
            fuelTypes,
            colors,
            transmissions
        );

        return Result<GetAvailableCarsFiltersValuesQueryResponse>.Success(response);
    }
}
