using Microsoft.Extensions.Logging;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Domain.Abstractions;

namespace RoyalRent.Application.Cars.Queries.GetAvailableCars;

public class GetAvailableCarsQueryHandler : IQueryHandler<GetAvailableCarsQuery, Result<List<GetAvailableCarsResponse>>>
{
    private readonly ICarsRepository _carsRepository;
    private readonly ILogger<GetAvailableCarsQueryHandler> _logger;

    public GetAvailableCarsQueryHandler(ICarsRepository carsRepository, ILogger<GetAvailableCarsQueryHandler> logger)
    {
        _carsRepository = carsRepository;
        _logger = logger;
    }

    public async Task<Result<List<GetAvailableCarsResponse>>> Handle(GetAvailableCarsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _carsRepository.GetAvailableCarsAsync();

        var availableCars = result.Select(car => new GetAvailableCarsResponse(
            car.Name,
            car.Category,
            car.Price,
            car.Seats,
            car.ImageUrl,
            car.Transmission,
            car.FuelType,
            car.Description
        )).ToList();

        _logger.LogInformation("Retrieved Available Cars {Cars}", availableCars);

        return Result<List<GetAvailableCarsResponse>>.Success(availableCars);
    }
}
