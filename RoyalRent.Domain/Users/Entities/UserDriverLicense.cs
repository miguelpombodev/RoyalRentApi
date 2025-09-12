using RoyalRent.Domain.Common.Entities;

namespace RoyalRent.Domain.Users.Entities;

public class UserDriverLicense : BaseEntity
{
    public UserDriverLicense(string rg, DateOnly birthDate, string driverLicenseNumber,
        DateOnly documentExpirationDate, string state, Guid userId)
    {
        Rg = rg;
        BirthDate = birthDate;
        DriverLicenseNumber = driverLicenseNumber;
        DocumentExpirationDate = documentExpirationDate;
        State = state;
        UserId = userId;
    }

    public string Rg { get; set; }
    public DateOnly BirthDate { get; set; }
    public string DriverLicenseNumber { get; set; }
    public DateOnly DocumentExpirationDate { get; set; }
    public string State { get; set; }
    public Guid UserId { get; set; }

    public User? User { get; set; }
}
