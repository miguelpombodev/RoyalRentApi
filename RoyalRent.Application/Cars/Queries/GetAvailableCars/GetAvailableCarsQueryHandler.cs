using Microsoft.Extensions.Logging;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Domain.Abstractions;
using RoyalRent.Domain.Abstractions.Filters;

namespace RoyalRent.Application.Cars.Queries.GetAvailableCars;

public class GetAvailableCarsQueryHandler : IQueryHandler<GetAvailableCarsQuery, Result<List<GetAvailableCarsResponse>>>
{
    private readonly ICarsRepository _carsRepository;

    public GetAvailableCarsQueryHandler(ICarsRepository carsRepository)
    {
        _carsRepository = carsRepository;
    }

    public async Task<Result<List<GetAvailableCarsResponse>>> Handle(GetAvailableCarsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _carsRepository.GetAvailableCarsAsync(request.Filters, request.Sort);

        var availableCars = result.Select(car => new GetAvailableCarsResponse(
            car.Name,
            car.Category,
            car.Price,
            car.Seats,
            car.ImageUrl,
            car.Transmission,
            car.FuelType,
            car.Description,
            car.Featured
        )).ToList();

        return Result<List<GetAvailableCarsResponse>>.Success(availableCars);
    }
}
