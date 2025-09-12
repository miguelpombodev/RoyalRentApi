namespace RoyalRent.Domain.Payments.Entities;

public sealed class IntentPaymentsInformation
{
    private string ClientSecret { get; set; } = string.Empty;
    private string Status { get; set; } = string.Empty;

    public IntentPaymentsInformation SetClientSecret(string clientSecret)
    {
        if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentException("Client Secret must have a value");
        ClientSecret = clientSecret.Trim();

        return this;
    }

    public IntentPaymentsInformation SetPaymentStatus(string status)
    {
        if (string.IsNullOrEmpty(status)) throw new ArgumentException("Payment status must have a value");
        Status = status;

        return this;
    }

    public string GetPaymentStatus()
    {
        return Status;
    }

    public IntentPaymentsInformation Build()
    {
        return this;
    }
}
