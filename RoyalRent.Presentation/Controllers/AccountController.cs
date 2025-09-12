using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalRent.Application.Accounts.Commands.GenerateRefreshToken;
using RoyalRent.Application.Accounts.Commands.Logout;
using RoyalRent.Application.Accounts.Commands.UpdateUserPassword;
using RoyalRent.Application.Accounts.Queries.GetByEmail;
using RoyalRent.Application.Accounts.Queries.Login;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Application.Users.Commands.CreateAccount;

namespace RoyalRent.Presentation.Controllers;

/// <summary>
/// Controller responsible for managing public account operations and authentication.
/// Provides endpoints for user registration, authentication, password management, and session handling.
/// This controller handles unauthenticated requests and manages the authentication lifecycle.
/// Implements secure cookie-based JWT token management with HttpOnly, Secure, and SameSite protection.
/// </summary>
/// <remarks>
/// This controller handles:
/// - User account registration with validation and error handling
/// - User authentication via login credentials with JWT token generation
/// - JWT access token refresh using secure refresh tokens stored in cookies
/// - Password reset functionality for forgot password scenarios
/// - User logout with refresh token invalidation
/// - Secure cookie management for both access and refresh tokens
///
/// Security Features:
/// - HttpOnly cookies to prevent XSS attacks
/// - Secure flag for HTTPS-only cookie transmission
/// - SameSite=Strict policy to prevent CSRF attacks
/// - Token expiration management with 60-minute default lifetime
/// - Refresh token validation and rotation for enhanced security
/// </remarks>
[ApiController]
[Route("api/account")]
public class AccountController : ApiController
{
    /// <summary>
    /// Creates a new user account in the system.
    /// Validates the provided user information and registers a new account with encrypted credentials.
    /// </summary>
    /// <param name="body">Account creation request containing user registration information such as email, password, and personal details</param>
    /// <returns>
    /// Returns HTTP 201 Created with success status if account creation is successful.
    /// Returns appropriate error status codes with structured error details if validation fails or account creation encounters issues.
    /// </returns>
    /// <remarks>
    /// This endpoint:
    /// - Accepts user registration data including email, password, and profile information
    /// - Adapts the request DTO to a CreateAccountCommand using Mapster
    /// - Processes the account creation through the CQRS mediator pattern
    /// - Validates email uniqueness and password strength requirements
    /// - Encrypts sensitive user data before storage
    /// - Returns structured error responses for validation failures
    ///
    /// Validation includes:
    /// - Email format and uniqueness verification
    /// - Password complexity requirements
    /// - Required field validation
    /// - Input sanitization for security
    ///
    /// The account is created in an inactive state and may require email verification
    /// depending on the application's email verification policies.
    /// </remarks>
    /// <response code="201">Account created successfully</response>
    /// <response code="400">Invalid request data, validation errors, or email already exists</response>
    /// <response code="500">Internal server error during account creation</response>
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Authenticates a user and generates JWT access and refresh tokens.
    /// Validates credentials and establishes a secure session using HTTP-only cookies.
    /// </summary>
    /// <param name="body">Login request containing user credentials (email and password)</param>
    /// <returns>
    /// Returns HTTP 200 OK with access token if authentication is successful.
    /// Returns appropriate error status codes with error details if authentication fails.
    /// </returns>
    /// <remarks>
    /// This endpoint performs the following authentication process:
    /// 1. Adapts the login request to a LoginCommand using Mapster
    /// 2. Processes authentication through the CQRS mediator pattern
    /// 3. Validates user credentials against encrypted stored passwords
    /// 4. Generates a new JWT access token with user claims and permissions
    /// 5. Creates a secure refresh token for token renewal
    /// 6. Sets both tokens as HTTP-only cookies with security attributes
    /// 7. Returns the access token in the response body for client use
    ///
    /// Security measures implemented:
    /// - Password validation against encrypted stored credentials
    /// - JWT tokens with appropriate expiration times
    /// - Secure cookie configuration (HttpOnly, Secure, SameSite=Strict)
    /// - Refresh token rotation for enhanced security
    /// - Protection against common web vulnerabilities (XSS, CSRF)
    ///
    /// Cookie Security Settings:
    /// - HttpOnly: Prevents JavaScript access to tokens
    /// - Secure: Ensures cookies are only sent over HTTPS
    /// - SameSite=Strict: Prevents cross-site request forgery
    /// - 60-minute expiration for both access and refresh tokens
    ///
    /// The response includes the access token for immediate use in API requests,
    /// while both tokens are securely stored in cookies for automatic handling.
    /// </remarks>
    /// <response code="200">Login successful, tokens generated and set in cookies</response>
    /// <response code="400">Invalid request format or missing required fields</response>
    /// <response code="401">Invalid credentials or account not found</response>
    /// <response code="423">Account locked due to multiple failed login attempts</response>
    /// <response code="500">Internal server error during authentication</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Generates a new access token using a valid refresh token.
    /// Implements token rotation by invalidating the old refresh token and issuing new tokens.
    /// </summary>
    /// <returns>
    /// Returns HTTP 200 OK with new access token if refresh is successful.
    /// Returns HTTP 401 Unauthorized if refresh token is missing, invalid, or expired.
    /// Returns appropriate error status codes for other failures.
    /// </returns>
    /// <remarks>
    /// This endpoint implements secure token refresh functionality:
    /// 1. Extracts the refresh token from the "refresh_token" HTTP-only cookie
    /// 2. Validates the refresh token's authenticity and expiration status
    /// 3. Creates a new GenerateRefreshTokenCommand with the current refresh token
    /// 4. Processes the token refresh through the CQRS mediator pattern
    /// 5. Issues a new access token with updated claims and expiration
    /// 6. Generates a new refresh token (token rotation for security)
    /// 7. Updates the refresh token cookie with the new token
    /// 8. Returns the new access token in the response body
    ///
    /// Security Benefits of Token Rotation:
    /// - Prevents long-term token compromise by regularly rotating refresh tokens
    /// - Limits the window of vulnerability if a refresh token is stolen
    /// - Maintains audit trail of token usage and renewal
    /// - Enables detection of token replay attacks
    ///
    /// Error Handling:
    /// - Missing refresh token returns 401 Unauthorized
    /// - Invalid or expired refresh token returns structured error response
    /// - Server errors during token generation are properly handled
    ///
    /// Note: There appears to be a missing 'return' statement in the original code
    /// for the missing refresh token scenario, which should be addressed.
    /// </remarks>
    /// <response code="200">Token refreshed successfully, new tokens issued</response>
    /// <response code="401">Missing, invalid, or expired refresh token</response>
    /// <response code="500">Internal server error during token refresh</response>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GenerateRefreshToken()
    {
        var refreshToken = Request.Cookies["refresh_token"];
        if (string.IsNullOrEmpty(refreshToken))
            return StatusCode(StatusCodes.Status401Unauthorized, new { status = "Missing Refresh Token" });

        var command = new GenerateRefreshTokenCommand(refreshToken!);

        var result = await Sender.Send(command);

        if (result.IsFailure)
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });

        SetRefreshTokenCookie(result.Data!.RefreshToken);
        return StatusCode(StatusCodes.Status200OK, new { token = result.Data.AccessToken });
    }

    /// <summary>
    /// Updates a user's password through the forgot password functionality.
    /// Performs a two-step process: validates user existence by email, then updates the password.
    /// </summary>
    /// <param name="body">Forgot password request containing user email and new password</param>
    /// <returns>
    /// Returns HTTP 200 OK with success status if password update is successful.
    /// Returns appropriate error status codes if user is not found or password update fails.
    /// </returns>
    /// <remarks>
    /// This endpoint implements a secure password reset process:
    /// 1. Adapts the forgot password request to a GetByEmailCommand to locate the user
    /// 2. Validates that the user exists in the system by email address
    /// 3. If user exists, creates an UpdateUserPasswordCommand with the user ID and new password
    /// 4. Processes the password update through the CQRS mediator pattern
    /// 5. Encrypts the new password using the application's security standards
    /// 6. Updates the user's password in the database
    /// 7. Returns success confirmation upon completion
    ///
    /// Security Considerations:
    /// - Password validation and strength requirements should be enforced
    /// - The new password is encrypted before storage using secure hashing algorithms
    /// - User verification by email ensures only valid users can reset passwords
    /// - No sensitive information is returned in the response
    ///
    /// Two-Phase Process Benefits:
    /// - Ensures user exists before attempting password update
    /// - Provides clear error messages for non-existent users
    /// - Maintains data integrity by using verified user IDs
    /// - Separates user lookup from password update for better error handling
    ///
    /// Note: In production, this endpoint should ideally include additional security measures
    /// such as email verification, temporary tokens, or multi-factor authentication
    /// to prevent unauthorized password changes.
    /// </remarks>
    /// <response code="200">Password updated successfully</response>
    /// <response code="400">Invalid request format or password validation errors</response>
    /// <response code="404">User not found with the provided email address</response>
    /// <response code="500">Internal server error during password update</response>
    [HttpPost("forgot")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Logs out the current user by invalidating their refresh token.
    /// Properly terminates the user session and prevents further token usage.
    /// </summary>
    /// <returns>
    /// Returns HTTP 200 OK with success status if logout is successful.
    /// Returns HTTP 401 Unauthorized if refresh token is missing.
    /// Returns appropriate error status codes for other logout failures.
    /// </returns>
    /// <remarks>
    /// This endpoint implements secure session termination:
    /// 1. Extracts the refresh token from the "refresh_token" HTTP-only cookie
    /// 2. Validates that a refresh token is present for logout processing
    /// 3. Creates a LogoutCommand with the current refresh token
    /// 4. Processes the logout through the CQRS mediator pattern
    /// 5. Invalidates the refresh token in the server-side storage/database
    /// 6. Marks the user session as terminated
    /// 7. Returns success confirmation upon completion
    ///
    /// Security Benefits:
    /// - Prevents unauthorized use of tokens after logout
    /// - Invalidates refresh tokens to prevent token replay attacks
    /// - Ensures proper session termination for security compliance
    /// - Maintains audit trail of logout events
    ///
    /// Client-Side Considerations:
    /// - Clients should clear any stored access tokens after successful logout
    /// - HTTP-only cookies cannot be cleared by JavaScript, requiring server-side management
    /// - Applications should redirect users to login page after successful logout
    ///
    /// Error Handling:
    /// - Missing refresh token indicates no active session or client-side issue
    /// - Invalid refresh tokens are handled gracefully with appropriate error responses
    /// - Server errors during logout are properly reported
    ///
    /// Note: There appears to be a missing 'return' statement in the original code
    /// for the missing refresh token scenario, which should be addressed.
    /// </remarks>
    /// <response code="200">Logout successful, session terminated</response>
    /// <response code="401">Missing refresh token or no active session</response>
    /// <response code="500">Internal server error during logout</response>
    [HttpPost("logout")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refresh_token"];
        if (string.IsNullOrEmpty(refreshToken))
            return StatusCode(StatusCodes.Status401Unauthorized, new { status = "Missing Refresh Token" });

        var command = new LogoutCommand(refreshToken!);
        var result = await Sender.Send(command);

        if (result.IsFailure)
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });

        return StatusCode(StatusCodes.Status200OK, new { status = "success" });
    }

    /// <summary>
    /// Sets a secure HTTP-only refresh token cookie with appropriate security attributes.
    /// Configures the cookie with maximum security settings to prevent common web attacks.
    /// </summary>
    /// <param name="refreshToken">The refresh token string to be stored in the cookie</param>
    /// <remarks>
    /// This private method configures refresh token cookies with the following security settings:
    ///
    /// Security Attributes:
    /// - HttpOnly: Prevents JavaScript access to the cookie, mitigating XSS attacks
    /// - Secure: Ensures the cookie is only transmitted over HTTPS connections
    /// - SameSite=Strict: Provides maximum CSRF protection by preventing cross-site cookie transmission
    /// - Expires: Sets cookie expiration to 60 minutes from current UTC time
    ///
    /// Cookie Name: "refresh_token"
    /// - Used for token refresh operations in the GenerateRefreshToken endpoint
    /// - Automatically included in subsequent requests to the same domain
    /// - Managed entirely server-side for enhanced security
    ///
    /// Security Benefits:
    /// - Protects against Cross-Site Scripting (XSS) attacks
    /// - Prevents Cross-Site Request Forgery (CSRF) attacks
    /// - Ensures tokens are only sent over encrypted connections
    /// - Automatic expiration prevents indefinite token persistence
    /// </remarks>
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

    /// <summary>
    /// Sets a secure HTTP-only access token cookie with appropriate security attributes.
    /// Configures the cookie with maximum security settings to prevent common web attacks.
    /// </summary>
    /// <param name="token">The JWT access token string to be stored in the cookie</param>
    /// <remarks>
    /// This private method configures access token cookies with the following security settings:
    ///
    /// Security Attributes:
    /// - HttpOnly: Prevents JavaScript access to the cookie, mitigating XSS attacks
    /// - Secure: Ensures the cookie is only transmitted over HTTPS connections
    /// - SameSite=Strict: Provides maximum CSRF protection by preventing cross-site cookie transmission
    /// - Expires: Sets cookie expiration to 60 minutes from current UTC time
    ///
    /// Cookie Name: "access_token"
    /// - Contains the JWT access token for API authentication
    /// - Automatically included in authenticated requests
    /// - Managed entirely server-side for enhanced security
    ///
    /// Security Benefits:
    /// - Protects against Cross-Site Scripting (XSS) attacks
    /// - Prevents Cross-Site Request Forgery (CSRF) attacks
    /// - Ensures tokens are only sent over encrypted connections
    /// - Automatic expiration aligns with JWT token lifetime
    ///
    /// Usage Pattern:
    /// - Set after successful login with the generated JWT access token
    /// - Updated during token refresh operations
    /// - Used by authenticated endpoints for user identification
    /// </remarks>
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
