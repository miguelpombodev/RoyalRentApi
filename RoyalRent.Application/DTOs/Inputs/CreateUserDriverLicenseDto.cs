using System.Text.Json.Serialization;

namespace RoyalRent.Application.DTOs.Inputs;

public record CreateUserDriverLicenseDto
{
    public CreateUserDriverLicenseDto(string rg, DateOnly birthDate, string driverLicenseNumber,
        DateOnly documentExpirationDate, string state)
    {
        Rg = rg;
        BirthDate = birthDate;
        DriverLicenseNumber = driverLicenseNumber;
        DocumentExpirationDate = documentExpirationDate;
        State = state;
        UserId = null;
    }

    public string Rg { get; set; }
    public DateOnly BirthDate { get; set; }
    public string DriverLicenseNumber { get; set; }
    public DateOnly DocumentExpirationDate { get; set; }
    public string State { get; set; }

    [JsonIgnore] public Guid? UserId { get; set; }
}
