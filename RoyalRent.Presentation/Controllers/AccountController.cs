using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.DTOs;
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

    public AccountController(IMapper mapper, IAccountHandler accountHandler)
    {
        _mapper = mapper;
        _accountHandler = accountHandler;
    }

    [HttpPost]
    public async Task<IActionResult> SaveAccount(CreateAccountRequest body)
    {
        var dto = _mapper.Map<CreateAccountDto>(body);

        var result = await _accountHandler.SaveAccountAsync(dto);

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });
        }

        return CreatedAtAction(nameof(AuthenticatedAccountController.GetAccountInformation),
            new { status = "success" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginAccountRequest body)
    {
        var result = await _accountHandler.Login(body);

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });
        }

        SetRefreshTokenCookie(result.Data!.RefreshToken);

        return StatusCode(StatusCodes.Status200OK, new { token = result.Data.AccessToken });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> GenerateRefreshToken()
    {
        var refreshToken = Request.Cookies["refresh_token"];
        if (string.IsNullOrEmpty(refreshToken))
            StatusCode(StatusCodes.Status401Unauthorized, new { status = "Missing Refresh Token" });

        var result = await _accountHandler.GenerateRefreshTokenHandler(refreshToken!);

        if (result.IsFailure)
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });

        SetRefreshTokenCookie(result.Data!.RefreshToken);
        return StatusCode(StatusCodes.Status200OK, new { token = result.Data.AccessToken });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refresh_token"];
        if (string.IsNullOrEmpty(refreshToken))
            StatusCode(StatusCodes.Status401Unauthorized, new { status = "Missing Refresh Token" });

        var result = await _accountHandler.LogoutHandler(refreshToken!);

        if (result.IsFailure)
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });

        return StatusCode(StatusCodes.Status200OK, new { status = "success" });
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        Response.Cookies.Append("refresh_token", refreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });
    }
}
