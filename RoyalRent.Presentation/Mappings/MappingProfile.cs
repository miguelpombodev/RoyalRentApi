using AutoMapper;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Application.DTOs.Outputs;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Users.Entities;

namespace RoyalRent.Presentation.Mappings;

/// <summary>
/// AutoMapper configuration profile for the presentation layer.
/// Defines object mapping configurations between DTOs, requests, and domain entities.
/// </summary>
/// <remarks>
/// Configures mappings for account operations including user creation, driver license management,
/// and response transformations. Uses constructor-based mapping for immutable DTOs.
/// </remarks>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes mapping configurations for presentation layer objects.
    /// Sets up constructor-based mappings between requests, DTOs, and domain entities.
    /// </summary>
    /// <remarks>
    /// Configured mappings:
    /// - CreateAccountRequest to CreateAccountDto with constructor mapping
    /// - CreateAccountDriverLicenseRequest to CreateUserDriverLicenseDto with constructor mapping
    /// - User entity to GetUserResponse with constructor mapping
    ///
    /// Uses ConstructUsing for immutable DTO patterns and proper data encapsulation.
    /// </remarks>
    public MappingProfile()
    {
        CreateMap<CreateAccountRequest, CreateAccountDto>().ConstructUsing(body => new CreateAccountDto(
            body.Name, body.Cpf, body.Email, body.Telephone, body.Gender, body.Password
        ));
        CreateMap<CreateAccountDriverLicenseRequest, CreateUserDriverLicenseDto>()
            .ConstructUsing(body => new CreateUserDriverLicenseDto(body.Rg, body.BirthDate, body.DriverLicenseNumber,
                body.DocumentExpirationDate, body.State));
        CreateMap<User, GetUserResponse>().ConstructUsing(user =>
            new GetUserResponse(user.Name, user.Cpf, user.Email, user.Telephone, user.Gender));
    }
}
