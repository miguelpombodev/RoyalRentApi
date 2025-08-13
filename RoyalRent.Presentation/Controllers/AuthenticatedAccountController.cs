using System.Text;
using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.Accounts.Commands.CreateDriverLicense;
using RoyalRent.Application.Accounts.Queries.GetByEmail;
using RoyalRent.Application.Accounts.Queries.GetUserDriverLicenseById;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Application.DTOs.Outputs;
using RoyalRent.Domain.Entities;
using RoyalRent.Presentation.Abstractions;
using RoyalRent.Presentation.Attributes;

namespace RoyalRent.Presentation.Controllers;

[ApiController]
[Route("api/account")]
[Authorize]
public class AuthenticatedAccountController : ApiController
{
    private readonly ICookiesHandler _cookiesHandler;
    private readonly IMapper _mapper;

    private const string GetCachedUserKeyPrefix = "user_cached:";
    private const string GetCachedUserLicenseKeyPrefix = "user_license_cached:";

    public AuthenticatedAccountController(IMapper mapper, ICookiesHandler cookiesHandler)
    {
        _mapper = mapper;
        _cookiesHandler = cookiesHandler;
    }

    [HttpPost("driver_license")]
    [ServiceFilter(typeof(CookiesHandlerAttribute))]
    public async Task<IResult> SaveAccountDriverLicense(CreateUserDriverLicenseDto body)
    {
        var userEmail = _cookiesHandler.ExtractJwtTokenFromCookie(Request.Cookies);
        var command = (body, userEmail).Adapt<CreateDriverLicenseCommand>();

        await Sender.Send(command);

        return Results.CreatedAtRoute("GetDriverLicense");
    }

    /// <summary>
    /// Gets user information
    /// </summary>
    /// <returns>The user specified by identifier, if exists</returns>
    [HttpGet(Name = "GetAccount")]
    [RedisCache(GetCachedUserKeyPrefix)]
    [ServiceFilter(typeof(CookiesHandlerAttribute))]
    public async Task<IActionResult> GetAccountInformation()
    {
        var userEmail = Encoding.Default.GetString(HttpContext.Session.Get("session_token")!);

        var query = new GetByEmailCommand(userEmail);

        var result = await Sender.Send(query);

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });
        }

        var mappedUserResponse = _mapper.Map<GetUserResponse>(result.Data);


        return StatusCode(StatusCodes.Status200OK, new { user = mappedUserResponse });
    }

    [HttpGet("license", Name = "GetDriverLicense")]
    [RedisCache(GetCachedUserLicenseKeyPrefix)]
    public async Task<IActionResult> GetAccountDriverLicenseInformation()
    {
        var userEmail = _cookiesHandler.ExtractJwtTokenFromCookie(Request.Cookies);

        var query = new GetByEmailCommand(userEmail);

        var resultGetUserByEmailQuery = await Sender.Send(query);

        if (resultGetUserByEmailQuery.IsFailure)
        {
            return StatusCode(resultGetUserByEmailQuery.Error.StatusCode,
                new
                {
                    error = new
                    {
                        ErrorCode = resultGetUserByEmailQuery.Error.Code,
                        resultGetUserByEmailQuery.Error.Description
                    }
                });
        }

        var getDriverLicenseQuery = new GetUserDriverLicenseByIdCommand(resultGetUserByEmailQuery.Data!.Id);
        var resultGetDriverLicenseQuery = await Sender.Send(getDriverLicenseQuery);

        if (resultGetDriverLicenseQuery.IsFailure)
        {
            return StatusCode(resultGetDriverLicenseQuery.Error.StatusCode,
                new
                {
                    error = new
                    {
                        ErrorCode = resultGetDriverLicenseQuery.Error.Code,
                        resultGetDriverLicenseQuery.Error.Description
                    }
                });
        }

        return StatusCode(StatusCodes.Status200OK, new { user = resultGetDriverLicenseQuery.Data });
    }
}
