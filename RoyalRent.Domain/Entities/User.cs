namespace RoyalRent.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Telephone { get; set; } = default!;
    public string? Gender { get; set; } = default;

    public UserAddress? UserAddress { get; set; }
    public UserDriverLicense? UserDriverLicense { get; set; }
}
