namespace RoyalRent.Domain.Payments;

public class PaymentRequest
{
    public long Amount { get; init; }
    public string Currency { get; init; } = "brl";
    public Dictionary<string, string> Metadata { get; init; } = new();
    public List<string> PaymentMethods { get; init; } = new() { "card" };
}
