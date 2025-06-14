using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Cars;
using RoyalRent.Presentation.Abstractions;
using RoyalRent.Presentation.Cars.Requests;

namespace RoyalRent.Presentation.Handlers;

public class CarHandler : ICarHandler
{
    private readonly ICreateCarService _createCarService;

    public CarHandler(ICreateCarService createCarService)
    {
        _createCarService = createCarService;
    }

    public async Task<Result<string>> InsertCarsDataByCsvFile(InsertFromCsvFileRequest body)
    {
        var result = await _createCarService.InsertCarsDataByCsvFile(body.File);

        return result;
    }
}
