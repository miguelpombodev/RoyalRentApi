using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalRent.Application.Accounts.Errors;
using RoyalRent.Application.DTOs;
using RoyalRent.Infrastructure.Abstractions;
using RoyalRent.Presentation.Abstractions;
using RoyalRent.Presentation.Accounts.Requests;
using RoyalRent.Presentation.Accounts.Responses;

namespace RoyalRent.Presentation.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IAccountHandler _accountHandler;
    private readonly IMapper _mapper;
    private readonly IDistribuitedCacheService _cacheService;

    public AccountController(IMapper mapper, IAccountHandler accountHandler, IDistribuitedCacheService cacheService)
    {
        _mapper = mapper;
        _accountHandler = accountHandler;
        _cacheService = cacheService;
    }

    [HttpPost]
    public async Task<IResult> SaveAccount(CreateAccountRequest body)
    {
        var dto = _mapper.Map<CreateAccountDto>(body);

        await _accountHandler.SaveAccountAsync(dto);

        return Results.CreatedAtRoute("GetAccount");
    }

    /// <summary>
    /// Gets user information
    /// </summary>
    /// <returns>The user specified by identifier, if exists</returns>
    [HttpGet(Name = "GetAccount")]
    public async Task<IActionResult> GetAccountInformation(Guid id)
    {
        var cachedUserResult = _cacheService.GetData<GetUserResponse>($"user_cached_{id}");

        if (cachedUserResult is not null)
        {
            return StatusCode(StatusCodes.Status200OK, new { user = cachedUserResult });
        }

        var result = await _accountHandler.GetUserInformationAsync(id);

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });
        }

        var mappedUserResponse = _mapper.Map<GetUserResponse>(result.Data);

        _cacheService.SetData($"user_cached_{id}", cachedUserResult);

        return StatusCode(StatusCodes.Status200OK, new { user = mappedUserResponse });
    }
}
