using System.Text;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalRent.Application.Accounts.Queries.GetByEmail;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Application.Rents.Commands.CreateRentCommand;
using RoyalRent.Application.Rents.DTOs;
using RoyalRent.Presentation.Abstractions;
using RoyalRent.Presentation.Attributes;

namespace RoyalRent.Presentation.Controllers;

/// <summary>
///
/// </summary>
[ApiController]
[Route("api/rent")]
[Authorize]
public class RentController : ApiController
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilter(typeof(CookiesHandlerAttribute))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRent([FromBody] CreateRentRequest body)
    {
        var userEmail = Encoding.Default.GetString(HttpContext.Session.Get("session_token")!);

        var query = new GetByEmailCommand(userEmail);

        var getUserResult = await Sender.Send(query);

        if (getUserResult.IsFailure)
        {
            return StatusCode(getUserResult.Error.StatusCode,
                new { error = new { ErrorCode = getUserResult.Error.Code, getUserResult.Error.Description } });
        }

        body = body with { UserId = getUserResult.Data!.Id };

        var command = body.Adapt<CreateRentCommand>();
        var createRentCommandResult = await Sender.Send(command);

        if (createRentCommandResult.IsFailure)
        {
            return StatusCode(createRentCommandResult.Error.StatusCode,
                new
                {
                    error = new
                    {
                        ErrorCode = createRentCommandResult.Error.Code,
                        createRentCommandResult.Error.Description
                    }
                });
        }

        return StatusCode(StatusCodes.Status200OK, createRentCommandResult.Data);
    }
}
