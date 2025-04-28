namespace RoyalRent.Application.Abstractions.Providers;

public interface IPasswordHasherProvider
{
    string Hash(string password);
    bool Verify(string password, string hashedPassword);
}
