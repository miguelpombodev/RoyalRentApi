namespace RoyalRent.Application.DTOs.Inputs;

public record CreateAccountDriverLicenseRequest(
    string Rg,
    DateOnly BirthDate,
    string DriverLicenseNumber,
    DateOnly DocumentExpirationDate,
    string State);
