namespace RoyalRent.Domain.Entities;

public class UserAddress : BaseEntity
{
    public string? Cep { get; set; } = default!;
    public string? Address { get; set; } = default!;
    public string? Number { get; set; } = default!;
    public string? Neighborhood { get; set; } = default!;
    public string? City { get; set; } = default!;
    public string? FederativeUnit { get; set; } = default!;
    public Guid? UserId { get; set; } = default!;

    public User? User { get; set; }
}
