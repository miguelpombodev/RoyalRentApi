namespace RoyalRent.Infrastructure.Abstractions;

public interface IEncryptionProviderService
{
    string Hash(string value);
    string Decrypt(string value);
}
