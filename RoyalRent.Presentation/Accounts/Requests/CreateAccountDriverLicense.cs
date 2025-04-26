namespace RoyalRent.Presentation.Accounts.Requests;

public record CreateAccountDriverLicenseRequest(
    string Rg,
    DateOnly BirthDate,
    string DriverLicenseNumber,
    DateOnly DocumentExpirationDate,
    string State);
