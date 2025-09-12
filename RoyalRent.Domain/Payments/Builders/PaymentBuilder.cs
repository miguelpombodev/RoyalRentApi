namespace RoyalRent.Domain.Payments.Builders;

public class PaymentBuilder
{
    private long _amount;
    private Dictionary<string, string> _metadata = new();
    private readonly string _brlCurrencyName = "brl";
    private readonly List<string> _paymentMethodsTypes = ["card"];

    public PaymentBuilder SetAmount(decimal amount)
    {
        var amountInCents = amount * 100m;
        _amount = Convert.ToInt64(amountInCents);
        return this;
    }

    public PaymentBuilder SetMetadata(Guid rentId, Guid carId, Guid userId)
    {
        _metadata = new Dictionary<string, string>
        {
            { "rent_id", rentId.ToString() }, { "car_id", carId.ToString() }, { "user_id", userId.ToString() }
        };
        return this;
    }

    private long GetAmount()
    {
        return _amount;
    }

    private Dictionary<string, string> GetMetadata()
    {
        return _metadata;
    }


    public PaymentRequest Build()
    {
        if (GetAmount() <= 0) throw new ArgumentException("Payment amount must be greater than zero");

        if (GetMetadata().Equals(null)) throw new ArgumentException("Metadata values cannot be null");

        return new PaymentRequest
        {
            Amount = GetAmount(),
            Currency = _brlCurrencyName,
            Metadata = GetMetadata(),
            PaymentMethods = _paymentMethodsTypes
        };
    }
}
