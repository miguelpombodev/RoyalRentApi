namespace RoyalRent.Domain.Entities;

public class UserPassword : BaseEntity
{
    public UserPassword(string passwordHashed, Guid userId, bool actualPassword)
    {
        PasswordHashed = passwordHashed;
        UserId = userId;
        ActualPassword = actualPassword;
    }

    public string PasswordHashed { get; set; }
    public Guid UserId { get; set; }
    public bool ActualPassword { get; set; }
    public User? User { get; set; }
}
