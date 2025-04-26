namespace RoyalRent.Application.DTOs;

public record CreateUserDriverLicenseDto(
    string Rg,
    DateOnly BirthDate,
    string DriverLicenseNumber,
    DateOnly DocumentExpirationDate,
    string State,
    Guid UserId);
