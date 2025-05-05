using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Domain.Entities;
using RoyalRent.Presentation.Abstractions;
using RoyalRent.Presentation.Accounts.Responses;

namespace RoyalRent.Presentation.Controllers;

[ApiController]
[Route("api/account")]
[Authorize]
public class AuthenticatedAccountController : ControllerBase
{
    private readonly IAccountHandler _accountHandler;
    private readonly ICookiesHandler _cookiesHandler;
    private readonly IMapper _mapper;
    private readonly IDistribuitedCacheProvider _cacheProvider;

    private const string GetCachedUserKeyPrefix = "user_cached:";
    private const string GetCachedUserLicenseKeyPrefix = "user_license_cached:";

    public AuthenticatedAccountController(IMapper mapper, IAccountHandler accountHandler,
        IDistribuitedCacheProvider cacheProvider, ICookiesHandler cookiesHandler)
    {
        _mapper = mapper;
        _accountHandler = accountHandler;
        _cacheProvider = cacheProvider;
        _cookiesHandler = cookiesHandler;
    }

    [HttpPost("driver_license")]
    public async Task<IResult> SaveAccountDriverLicense(CreateUserDriverLicenseDto body)
    {
        var userEmail = _cookiesHandler.ExtractJwtTokenFromCookie(Request.Cookies);
        var dto = _mapper.Map<CreateUserDriverLicenseDto>(body);

        await _accountHandler.SaveDriverLicenseHandler(dto, userEmail);

        return Results.CreatedAtRoute("GetDriverLicense");
    }

    /// <summary>
    /// Gets user information
    /// </summary>
    /// <returns>The user specified by identifier, if exists</returns>
    [HttpGet(Name = "GetAccount")]
    public async Task<IActionResult> GetAccountInformation()
    {
        var userEmail = _cookiesHandler.ExtractJwtTokenFromCookie(Request.Cookies);

        var cachedUserResult =
            _cacheProvider.GetData<GetUserResponse>(string.Concat(GetCachedUserKeyPrefix, userEmail));

        if (cachedUserResult is not null)
        {
            return StatusCode(StatusCodes.Status200OK, new { user = cachedUserResult });
        }

        var result = await _accountHandler.GetUserInformationHandler(userEmail);

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });
        }

        var mappedUserResponse = _mapper.Map<GetUserResponse>(result.Data);

        _cacheProvider.SetData(string.Concat(GetCachedUserKeyPrefix, userEmail), cachedUserResult);

        return StatusCode(StatusCodes.Status200OK, new { user = mappedUserResponse });
    }

    [HttpGet("/license", Name = "GetDriverLicense")]
    public async Task<IActionResult> GetAccountDriverLicenseInformation()
    {
        var userEmail = _cookiesHandler.ExtractJwtTokenFromCookie(Request.Cookies);

        var cachedUserResult =
            _cacheProvider.GetData<UserDriverLicense>(string.Concat(GetCachedUserKeyPrefix, userEmail));

        if (cachedUserResult is not null)
        {
            return StatusCode(StatusCodes.Status200OK, new { user = cachedUserResult });
        }

        var result = await _accountHandler.GetUserDriverLicenseHandler(userEmail);

        _cacheProvider.SetData(string.Concat(GetCachedUserLicenseKeyPrefix, userEmail), cachedUserResult);

        return StatusCode(StatusCodes.Status200OK, new { user = result.Data });
    }
}
