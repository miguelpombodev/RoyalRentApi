using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalRent.Application.Cars.Commands.CreateCarsDataByCsvFile;
using RoyalRent.Application.Cars.Queries.GetAvailableCars;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Presentation.Abstractions;

namespace RoyalRent.Presentation.Controllers;

[ApiController]
[Route("api/cars")]
public class CarCommandController : ApiController
{
    private readonly ICookiesHandler _cookiesHandler;

    public CarCommandController(ICookiesHandler cookiesHandler)
    {
        _cookiesHandler = cookiesHandler;
    }

    [HttpPost("populate")]
    [Authorize]
    public async Task<IActionResult> UploadCarsDataFromCsvFile(
        [FromForm] InsertFromCsvFileRequest body)
    {
        _cookiesHandler.ExtractJwtTokenFromCookie(Request.Cookies);
        var command = body.Adapt<CreateCarsDataByCsvFileCommand>();

        var result = await Sender.Send(command);

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
public class CarQueryController : ApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllAvailableCarsToRentAsync(CancellationToken cancellationToken)
    {
        var query = new GetAvailableCarsQuery();
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });

        return StatusCode(StatusCodes.Status200OK, new { result.Data });
    }
}
