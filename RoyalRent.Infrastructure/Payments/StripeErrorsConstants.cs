
namespace RoyalRent.Infrastructure.Payments;

public static class StripeErrorsConstants
{
    public const string DeclinedCard = "card_declined";
    public const string BlockedCard = "blocked";
    public const string GenericCardError = "card_error";
}
