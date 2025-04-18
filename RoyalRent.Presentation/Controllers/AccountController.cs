using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.DTOs;
using RoyalRent.Presentation.Accounts.Requests;

namespace RoyalRent.Presentation.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly ICreateAccountService _createAccountService;
    private readonly IMapper _mapper;

    public AccountController(IMapper mapper, ICreateAccountService createAccountService)
    {
        _mapper = mapper;
        _createAccountService = createAccountService;
    }
    /// <summary>
    /// Gets user information
    /// </summary>
    /// <returns>The user specified by identifier, if exists</returns>
    [HttpGet]
    public IActionResult GetUser()
    {
        return StatusCode(StatusCodes.Status200OK, new { name = "Jonh Doe" });
    }


    [HttpPost]
    public async Task<IActionResult> SaveAccount(CreateAccountRequest body)
    {
        var dto = _mapper.Map<CreateAccountDto>(body);

        var user = await _createAccountService.ExecuteAsync(dto);

        return StatusCode(StatusCodes.Status200OK, user);
    }
}
