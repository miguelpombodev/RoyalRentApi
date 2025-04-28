namespace RoyalRent.Domain.Entities;

public class User : BaseEntity
{
    public User(string name, string cpf, string email, string telephone, char? gender = default)
    {
        Name = name;
        Cpf = cpf;
        Email = email;
        Telephone = telephone;
        Gender = gender;
    }

    public string Name { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public char? Gender { get; set; }
    public UserAddress? UserAddress { get; set; }
    public UserDriverLicense? UserDriverLicense { get; set; }
    public ICollection<UserPassword> UserPasswords { get; set; } = new List<UserPassword>();
}
