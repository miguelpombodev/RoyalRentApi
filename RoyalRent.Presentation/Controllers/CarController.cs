using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalRent.Application.Abstractions.Cars;
using RoyalRent.Presentation.Abstractions;
using RoyalRent.Presentation.Cars.Requests;

namespace RoyalRent.Presentation.Controllers;

[ApiController]
[Route("api/cars")]
public class CarCommandController : ControllerBase
{
    private readonly ICarCommandService _carCommandService;
    private readonly ICookiesHandler _cookiesHandler;

    public CarCommandController(ICookiesHandler cookiesHandler, ICarCommandService carCommandService)
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

[ApiController]
[Route("api/cars")]
public class CarQueryController : ControllerBase
{
    private readonly ICarQueryService _carQueryService;

    public CarQueryController(ICarQueryService carQueryService)
    {
        _carQueryService = carQueryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAvailableCarsToRentAsync()
    {
        var result = await _carQueryService.GetAvailableCarsAsync();

        if (result.IsFailure)
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });

        return StatusCode(StatusCodes.Status200OK, new { result.Data });
    }
}
