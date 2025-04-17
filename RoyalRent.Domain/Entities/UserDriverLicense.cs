namespace RoyalRent.Domain.Entities;

public class UserDriverLicense : BaseEntity
{
    public string? RG { get; set; } = null;
    public DateOnly? BirthDate { get; set; } = null;
    public string? DriverLicenseNumber { get; set; } = null;
    public DateOnly? DocumentExpirationDate { get; set; } = null;
    public string? State { get; set; } = null;
    public Guid? UserId { get; set; } = default!;

    public User? User { get; set; }
}
