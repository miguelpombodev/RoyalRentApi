using System.Security.Cryptography;
using RoyalRent.Application.Abstractions.Providers;

namespace RoyalRent.Infrastructure.Hasher;

public sealed class PasswordHasherProvider : IPasswordHasherProvider
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;

    private const string PasswordHashDelimiter = "-";

    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return string.Concat(Convert.ToHexString(hash), PasswordHashDelimiter, Convert.ToHexString(salt));
    }

    public bool Verify(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split(PasswordHashDelimiter);
        byte[] hash = Convert.FromHexString(parts[0]);
        byte[] salt = Convert.FromHexString(parts[1]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}
