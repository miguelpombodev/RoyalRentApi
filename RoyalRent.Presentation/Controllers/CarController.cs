using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalRent.Application.Abstractions.Cars;
using RoyalRent.Presentation.Abstractions;
using RoyalRent.Presentation.Cars.Requests;

namespace RoyalRent.Presentation.Controllers;

[ApiController]
[Route("api/cars")]
public class CarController : ControllerBase
{
    private readonly ICarCommandService _carCommandService;
    private readonly ICookiesHandler _cookiesHandler;
    public CarController(ICookiesHandler cookiesHandler, ICarCommandService carCommandService)
    {
        _cookiesHandler = cookiesHandler;
        _carCommandService = carCommandService;
    }

    [HttpPost("populate")]
    [Authorize]
    public async Task<IActionResult> UploadCarsDataFromCsvFile(
        [FromForm] InsertFromCsvFileRequest body)
    {
        _cookiesHandler.ExtractJwtTokenFromCookie(Request.Cookies);
        var result = await _carCommandService.InsertCarsDataByCsvFile(body.File);

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });
        }

        return Created("/api/account/", new { status = "success" });
    }


}
