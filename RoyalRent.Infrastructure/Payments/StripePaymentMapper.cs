using RoyalRent.Domain.Payments;
using Stripe;

namespace RoyalRent.Infrastructure.Payments;

public static class StripePaymentMapper
{
    public static PaymentIntentCreateOptions CreateOptions(PaymentRequest request)
    {
        return new PaymentIntentCreateOptions
        {
            Currency = request.Currency,
            Amount = request.Amount,
            Metadata = request.Metadata,
            PaymentMethodTypes = request.PaymentMethods
        };
    }
}
