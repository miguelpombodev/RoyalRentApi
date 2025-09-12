using System.Text.Json.Serialization;

namespace RoyalRent.Application.DTOs.Inputs;

public record CreateUserDriverLicenseDto(
    string Rg,
    DateOnly BirthDate,
    string DriverLicenseNumber,
    DateOnly DocumentExpirationDate,
    string State)
{
    public string Rg { get; set; } = Rg;
    public DateOnly BirthDate { get; set; } = BirthDate;
    public string DriverLicenseNumber { get; set; } = DriverLicenseNumber;
    public DateOnly DocumentExpirationDate { get; set; } = DocumentExpirationDate;
    public string State { get; set; } = State;

    [JsonIgnore] public Guid? UserId { get; set; } = null;
}
