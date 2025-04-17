using System.Net.Mail;

namespace RoyalRent.Domain.ValueObjects;

public record Email
{
    public Email(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value), "Email cannot be empty");
        }

        if (!IsValidEmail(value))
        {
            throw new ArgumentException("Invalid email format", nameof(value));
        }

        Value = value;
    }
    public string Value { get; init; }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new MailAddress(email);

            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }

}
