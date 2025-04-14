using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoyalRent.Presentation.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    /// <summary>
    /// Gets user informations
    /// </summary>
    /// <returns>The user specified by identifier, if exists</returns>
    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        return StatusCode(StatusCodes.Status200OK, new
        {
            name= "Jonh Doe"
        });
    }
}