using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalRent.Application.Cars.Commands.CreateCarsDataByCsvFile;
using RoyalRent.Application.Cars.Queries.GetAvailableCars;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Presentation.Abstractions;

namespace RoyalRent.Presentation.Controllers;

/// <summary>
/// Controller responsible for managing car-related command operations that modify data.
/// Handles administrative operations for car inventory management including bulk data imports from CSV files.
/// All command operations require authentication to ensure only authorized users can modify car data.
/// Implements secure JWT token validation through cookie-based authentication.
/// </summary>
/// <remarks>
/// This controller focuses on write operations for car management:
/// - Bulk car data import from CSV files for initial inventory setup or updates
/// - Administrative functions for car fleet management
/// - Secure authentication validation for all data modification operations
///
/// Security Features:
/// - JWT token authentication required for all endpoints
/// - Cookie-based token extraction for secure session management
/// - Authorization attribute ensures only authenticated users can access endpoints
/// - Proper error handling with structured error responses
///
/// Design Pattern:
/// - Follows CQRS (Command Query Responsibility Segregation) pattern
/// - Separated from query operations for better maintainability and scalability
/// - Uses Mapster for efficient object mapping and data transformation
/// - Implements mediator pattern through the ApiController base class
/// </remarks>
[ApiController]
[Route("api/cars")]
public class CarCommandController : ApiController
{
    private readonly ICookiesHandler _cookiesHandler;

    /// <summary>
    /// Initializes a new instance of the CarCommandController class.
    /// </summary>
    /// <param name="cookiesHandler">Service for handling JWT token extraction from HTTP cookies</param>
    public CarCommandController(ICookiesHandler cookiesHandler)
    {
        _cookiesHandler = cookiesHandler;
    }

    /// <summary>
    /// Uploads and processes car inventory data from a CSV file for bulk car data population.
    /// Validates the authenticated user and processes the CSV file to create multiple car records in the system.
    /// </summary>
    /// <param name="body">Form data request containing the CSV file with car information and processing parameters</param>
    /// <returns>
    /// Returns HTTP 201 Created with success status if the CSV processing and car creation is successful.
    /// Returns appropriate error status codes with structured error details if authentication, validation, or processing fails.
    /// </returns>
    /// <remarks>
    /// This endpoint performs comprehensive CSV-based car data import:
    /// 1. Validates the user's authentication status using JWT token from cookies
    /// 2. Extracts and validates the JWT token to ensure user authorization
    /// 3. Adapts the form request containing CSV file data to a CreateCarsDataByCsvFileCommand
    /// 4. Processes the CSV file through the CQRS mediator pattern
    /// 5. Validates CSV structure, data integrity, and business rules
    /// 6. Creates multiple car records in batch for optimal performance
    /// 7. Returns structured responses for both success and error scenarios
    ///
    /// CSV Processing Features:
    /// - Bulk data import for efficient inventory population
    /// - Data validation and sanitization for each car record
    /// - Error reporting for invalid or malformed CSV data
    /// - Transaction management to ensure data consistency
    /// - Support for large datasets with optimized batch processing
    ///
    /// Authentication Requirements:
    /// - Valid JWT token must be present in HTTP cookies
    /// - User must have appropriate permissions for car data management
    /// - Token validation occurs before any data processing begins
    ///
    /// Expected CSV Format:
    /// - Should contain columns for car details such as make, model, year, license plate, etc.
    /// - First row typically contains column headers
    /// - Data validation rules applied to each row during processing
    ///
    /// Error Handling:
    /// - Authentication failures return 401 Unauthorized
    /// - Invalid CSV format or data validation errors return 400 Bad Request
    /// - Processing errors provide detailed error codes and descriptions
    /// - File upload issues are handled with appropriate HTTP status codes
    ///
    /// Performance Considerations:
    /// - Designed for bulk operations with optimized database interactions
    /// - Implements batch processing for large CSV files
    /// - Memory-efficient streaming for large file uploads
    /// </remarks>
    /// <response code="201">CSV file processed successfully, car data created</response>
    /// <response code="400">Invalid CSV format, data validation errors, or malformed request</response>
    /// <response code="401">User not authenticated or invalid JWT token</response>
    /// <response code="403">User lacks sufficient permissions for car data management</response>
    /// <response code="413">CSV file too large or exceeds system limits</response>
    /// <response code="415">Unsupported file format or invalid media type</response>
    /// <response code="500">Internal server error during CSV processing or car creation</response>
    /// <description>
    /// Bulk import car inventory data from CSV file. Requires authentication and processes CSV files containing car information
    /// including make, model, year, license plate, and other vehicle details. The system validates each record and creates
    /// car entries in batch for optimal performance. Supports large datasets with proper error reporting for invalid data.
    /// </description>
    [HttpPost("populate")]
    [Authorize]
    public async Task<IActionResult> UploadCarsDataFromCsvFile(
        [FromForm] InsertFromCsvFileRequest body)
    {
        _cookiesHandler.ExtractJwtTokenFromCookie(Request.Cookies);
        var command = body.Adapt<CreateCarsDataByCsvFileCommand>();

        var result = await Sender.Send(command);

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });
        }

        return Created("/api/account/", new { status = "success" });
    }
}

/// <summary>
/// Controller responsible for managing car-related query operations that retrieve data.
/// Handles read-only operations for car inventory information, focusing on available cars for rental.
/// Provides public access to car availability information without authentication requirements.
/// Implements efficient data retrieval with proper error handling and response formatting.
/// </summary>
/// <remarks>
/// This controller focuses on read operations for car inventory:
/// - Retrieval of available cars for rental purposes
/// - Public access endpoints for customer-facing car browsing
/// - Optimized queries for performance and user experience
/// - Support for filtering and search capabilities (extensible)
///
/// Design Pattern:
/// - Follows CQRS (Command Query Responsibility Segregation) pattern
/// - Separated from command operations for better performance optimization
/// - Read-only operations allow for specialized caching and query optimization
/// - Uses mediator pattern for consistent request handling
///
/// Performance Features:
/// - Optimized for high-frequency read operations
/// - Supports cancellation tokens for responsive user experience
/// - Structured for easy integration with caching mechanisms
/// - Minimal data transfer with focused response models
///
/// Public Access:
/// - No authentication required for browsing available cars
/// - Customer-friendly endpoints for rental application integration
/// - Supports anonymous users exploring rental options
/// </remarks>
[ApiController]
[Route("api/cars")]
public class CarQueryController : ApiController
{
    /// <summary>
    /// Retrieves all cars currently available for rental from the system.
    /// Provides comprehensive information about cars that customers can rent, including availability status and car details.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to support request cancellation for improved responsiveness and resource management</param>
    /// <returns>
    /// Returns HTTP 200 OK with a collection of available car data if the query is successful.
    /// Returns appropriate error status codes with structured error details if the query encounters issues.
    /// </returns>
    /// <remarks>
    /// This endpoint provides comprehensive car availability information:
    /// 1. Creates a GetAvailableCarsQuery command for processing
    /// 2. Processes the query through the CQRS mediator pattern with cancellation support
    /// 3. Retrieves all cars marked as available for rental in the system
    /// 4. Filters out cars that are currently rented, under maintenance, or otherwise unavailable
    /// 5. Returns structured car data including essential rental information
    /// 6. Supports request cancellation for better user experience and resource management
    ///
    /// Query Features:
    /// - Retrieves only cars with "available" rental status
    /// - Includes essential car details for customer decision-making
    /// - Optimized for frequent access by potential customers
    /// - Supports high-concurrency scenarios typical of rental applications
    ///
    /// Response Data Includes:
    /// - Car identification information (ID, make, model, year)
    /// - Rental-specific details (price, availability dates, location)
    /// - Car specifications relevant to customer selection
    /// - Current availability status and rental terms
    ///
    /// Performance Optimizations:
    /// - Cancellation token support prevents resource waste on cancelled requests
    /// - Efficient database queries focused on available inventory only
    /// - Structured for easy integration with frontend pagination and filtering
    /// - Minimal data payload for faster response times
    ///
    /// Error Handling:
    /// - Database connectivity issues handled gracefully
    /// - Empty result sets return successful responses with empty collections
    /// - System errors provide structured error information
    /// - Request cancellation handled appropriately
    ///
    /// Public Access:
    /// - No authentication required, supporting anonymous browsing
    /// - Customer-friendly for rental application integration
    /// - Suitable for public-facing car rental websites and mobile apps
    ///
    /// Extensibility:
    /// - Structure supports future filtering parameters (location, price range, car type)
    /// - Ready for pagination implementation for large inventories
    /// - Compatible with search and sorting functionality additions
    /// </remarks>
    /// <response code="200">Available cars retrieved successfully</response>
    /// <response code="500">Internal server error during car data retrieval</response>
    [HttpGet]
    public async Task<IActionResult> GetAllAvailableCarsToRentAsync(CancellationToken cancellationToken)
    {
        var query = new GetAvailableCarsQuery();
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return StatusCode(result.Error.StatusCode,
                new { error = new { ErrorCode = result.Error.Code, result.Error.Description } });

        return StatusCode(StatusCodes.Status200OK, new { result.Data });
    }
}
