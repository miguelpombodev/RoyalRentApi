using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalRent.Application.Accounts.Commands.CreateAccount;
using RoyalRent.Application.Accounts.Commands.GenerateRefreshToken;
using RoyalRent.Application.Accounts.Commands.Logout;
using RoyalRent.Application.Accounts.Commands.UpdateUserPassword;
using RoyalRent.Application.Accounts.Queries.GetByEmail;
using RoyalRent.Application.Accounts.Queries.Login;
using RoyalRent.Application.DTOs.Inputs;

namespace RoyalRent.Presentation.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ApiController
{
    [HttpPost]
    public async Task<IActionResult> SaveAccount(CreateAccountRequest body)
    {
        var command = body.Adapt<CreateAccountCommand>();

        var result = await Sender.Send(command);

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });
        }

        return Created("/api/account/", new { status = "success" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginAccountRequest body)
    {
        var command = body.Adapt<LoginCommand>();

        var result = await Sender.Send(command);

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });
        }

        SetJwtTokenCookie(result.Data!.AccessToken);
        SetRefreshTokenCookie(result.Data!.RefreshToken);

        return StatusCode(StatusCodes.Status200OK, new { token = result.Data.AccessToken });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> GenerateRefreshToken()
    {
        var refreshToken = Request.Cookies["refresh_token"];
        if (string.IsNullOrEmpty(refreshToken))
            StatusCode(StatusCodes.Status401Unauthorized, new { status = "Missing Refresh Token" });

        var command = new GenerateRefreshTokenCommand(refreshToken!);

        var result = await Sender.Send(command);

        if (result.IsFailure)
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });

        SetRefreshTokenCookie(result.Data!.RefreshToken);
        return StatusCode(StatusCodes.Status200OK, new { token = result.Data.AccessToken });
    }

    [HttpPost("forgot")]
    public async Task<IActionResult> UpdateUserForgotPassword(ForgotPasswordRequest body)
    {
        var query = body.Adapt<GetByEmailCommand>();
        var resultQuery = await Sender.Send(query);

        if (resultQuery.IsFailure)
            return StatusCode(resultQuery.Error.StatusCode,
                new { error = new { ErrorCode = resultQuery.Error.Code, resultQuery.Error.Description } });

        var command = new UpdateUserPasswordCommand(resultQuery.Data!.Id, body.NewPassword);
        var resultCommand = await Sender.Send(command);

        if (resultCommand.IsFailure)
            return StatusCode(resultCommand.Error.StatusCode,
                new { error = new { ErrorCode = resultCommand.Error.Code, resultCommand.Error.Description } });

        return StatusCode(StatusCodes.Status200OK, new { status = "success" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refresh_token"];
        if (string.IsNullOrEmpty(refreshToken))
            StatusCode(StatusCodes.Status401Unauthorized, new { status = "Missing Refresh Token" });

        var command = new LogoutCommand(refreshToken!);
        var result = await Sender.Send(command);

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

    private void SetJwtTokenCookie(string token)
    {
        Response.Cookies.Append("access_token", token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });
    }
}
