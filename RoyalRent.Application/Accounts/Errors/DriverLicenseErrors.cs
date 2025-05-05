using RoyalRent.Application.Abstractions;

namespace RoyalRent.Application.Accounts.Errors;

public static class DriverLicenseErrors
{
    public static readonly Error UserDriverLicenseNotFound = new("UserDriverLicense.UserDriverLicenseNotFound", 404, "User driver license not found");
}
