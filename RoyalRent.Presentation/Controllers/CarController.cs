using Microsoft.AspNetCore.Mvc;
using RoyalRent.Presentation.Abstractions;
using RoyalRent.Presentation.Cars.Requests;

namespace RoyalRent.Presentation.Controllers;

[ApiController]
[Route("api/cars")]
public class CarController : ControllerBase
{
    private readonly ICarHandler _carHandler;

    public CarController(ICarHandler carHandler)
    {
        _carHandler = carHandler;
    }

    [HttpPost("populate")]
    public async Task<IActionResult> UploadCarsDataFromCsvFile(
        [FromForm] InsertFromCsvFileRequest body)
    {
        var result = await _carHandler.InsertCarsDataByCsvFile(body);

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });
        }

        return Created("/api/account/", new { status = "success" });
    }
}
