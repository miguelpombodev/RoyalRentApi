using System.Text;
using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.Accounts.Commands.CreateDriverLicense;
using RoyalRent.Application.Accounts.Queries.GetByEmail;
using RoyalRent.Application.Accounts.Queries.GetUserDriverLicenseById;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Application.DTOs.Outputs;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Users.Entities;
using RoyalRent.Presentation.Abstractions;
using RoyalRent.Presentation.Attributes;

namespace RoyalRent.Presentation.Controllers;

/// <summary>
/// Controller responsible for managing authenticated user account operations.
/// Provides endpoints for authenticated users to manage their account information and driver license data.
/// All endpoints require user authentication via JWT tokens stored in cookies.
/// Implements caching mechanisms using Redis to optimize performance for frequently accessed data.
/// </summary>
/// <remarks>
/// This controller handles:
/// - User account information retrieval with session-based authentication
/// - Driver license management (creation and retrieval) with cookie-based JWT authentication
/// - Redis caching for improved performance on data retrieval operations
/// - Error handling with appropriate HTTP status codes and structured error responses
/// </remarks>
[ApiController]
[Route("api/account")]
[Authorize]
public class AuthenticatedAccountController : ApiController
{
    private readonly ICookiesHandler _cookiesHandler;
    private readonly IMapper _mapper;

    /// <summary>
    /// Redis cache key prefix for storing cached user information
    /// </summary>
    private const string GetCachedUserKeyPrefix = "user_cached:";

    /// <summary>
    /// Redis cache key prefix for storing cached user driver license information
    /// </summary>
    private const string GetCachedUserLicenseKeyPrefix = "user_license_cached:";

    /// <summary>
    /// Initializes a new instance of the AuthenticatedAccountController class.
    /// </summary>
    /// <param name="mapper">AutoMapper instance for object-to-object mapping operations</param>
    /// <param name="cookiesHandler">Service for handling JWT token extraction from cookies</param>
    public AuthenticatedAccountController(IMapper mapper, ICookiesHandler cookiesHandler)
    {
        _mapper = mapper;
        _cookiesHandler = cookiesHandler;
    }

    /// <summary>
    /// Creates and saves a new driver license for the authenticated user.
    /// Extracts the user's email from JWT token stored in cookies and associates the license with the user account.
    /// </summary>
    /// <param name="body">Driver license creation data transfer object containing all required license information</param>
    /// <returns>
    /// Returns HTTP 201 Created with a location header pointing to the GetDriverLicense endpoint if successful.
    /// Returns appropriate error status codes with error details if the operation fails.
    /// </returns>
    /// <remarks>
    /// This endpoint:
    /// - Requires authentication via JWT token in cookies
    /// - Uses the CookiesHandlerAttribute service filter for token processing
    /// - Adapts the input DTO and user email into a CreateDriverLicenseCommand using Mapster
    /// - Sends the command through the CQRS mediator pattern
    /// - Returns a Created response with route reference for resource location
    ///
    /// Expected request body should include all necessary driver license fields such as:
    /// license number, expiration date, issuing authority, etc.
    /// </remarks>
    /// <response code="201">Driver license created successfully</response>
    /// <response code="400">Invalid request data or validation errors</response>
    /// <response code="401">User not authenticated or invalid JWT token</response>
    /// <response code="500">Internal server error during license creation</response>
    [HttpPost("driver_license")]
    [ServiceFilter(typeof(CookiesHandlerAttribute))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> SaveAccountDriverLicense(CreateUserDriverLicenseDto body)
    {
        var userEmail = _cookiesHandler.ExtractJwtTokenFromCookie(Request.Cookies);
        var command = (body, userEmail).Adapt<CreateDriverLicenseCommand>();

        await Sender.Send(command);

        return Results.CreatedAtRoute("GetDriverLicense");
    }

    /// <summary>
    /// Retrieves comprehensive account information for the authenticated user.
    /// Uses session-based authentication to identify the user and returns cached data when available.
    /// </summary>
    /// <returns>
    /// Returns HTTP 200 OK with user information if successful.
    /// Returns appropriate error status codes with structured error details if the operation fails.
    /// </returns>
    /// <remarks>
    /// This endpoint:
    /// - Requires authentication via session token stored in HttpContext.Session
    /// - Implements Redis caching using the RedisCache attribute with "user_cached:" prefix
    /// - Uses CookiesHandlerAttribute service filter for additional cookie processing
    /// - Extracts user email from session token stored as bytes in session storage
    /// - Queries user data using CQRS pattern with GetByEmailCommand
    /// - Maps domain entities to response DTOs using AutoMapper
    /// - Returns structured error responses with error codes and descriptions
    ///
    /// The response includes comprehensive user account information but excludes sensitive data.
    /// Cached responses improve performance for frequently requested user data.
    /// </remarks>
    /// <response code="200">User information retrieved successfully</response>
    /// <response code="401">User not authenticated or invalid session token</response>
    /// <response code="404">User not found in the system</response>
    /// <response code="500">Internal server error during user retrieval</response>
    [HttpGet(Name = "GetAccount")]
    [RedisCache(GetCachedUserKeyPrefix)]
    [ServiceFilter(typeof(CookiesHandlerAttribute))]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        return StatusCode(StatusCodes.Status200OK, mappedUserResponse);
    }

    /// <summary>
    /// Retrieves driver license information for the authenticated user.
    /// Performs a two-step process: first retrieves user information, then fetches associated driver license data.
    /// Implements Redis caching for optimal performance on license data retrieval.
    /// </summary>
    /// <returns>
    /// Returns HTTP 200 OK with driver license information if successful.
    /// Returns appropriate error status codes with structured error details if the operation fails.
    /// </returns>
    /// <remarks>
    /// This endpoint performs the following operations:
    /// 1. Extracts user email from JWT token stored in cookies using ICookiesHandler
    /// 2. Queries user information by email to obtain the user ID
    /// 3. Uses the user ID to retrieve associated driver license information
    /// 4. Implements Redis caching with "user_license_cached:" prefix for performance optimization
    /// 5. Returns structured error responses for both user lookup and license retrieval failures
    ///
    /// The two-step process ensures data integrity and proper authorization:
    /// - First step validates the user exists and is properly authenticated
    /// - Second step retrieves license data using the verified user ID
    ///
    /// Error handling includes:
    /// - User not found errors from the initial email lookup
    /// - Driver license not found errors from the license query
    /// - Structured error responses with specific error codes and descriptions
    /// - Appropriate HTTP status codes reflecting the nature of each error
    ///
    /// Caching strategy improves response times for frequently accessed license information
    /// while ensuring data freshness through appropriate cache invalidation policies.
    /// </remarks>
    /// <response code="200">Driver license information retrieved successfully</response>
    /// <response code="401">User not authenticated or invalid JWT token</response>
    /// <response code="404">User or driver license not found</response>
    /// <response code="500">Internal server error during license retrieval</response>
    [HttpGet("license", Name = "GetDriverLicense")]
    [RedisCache(GetCachedUserLicenseKeyPrefix)]
    [ProducesResponseType(typeof(UserDriverLicense), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAccountDriverLicenseInformation()
    {
        var userEmail = await _cookiesHandler.ExtractJwtTokenFromCookie(Request.Cookies);

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

        return StatusCode(StatusCodes.Status200OK, resultGetDriverLicenseQuery.Data);
    }
}
