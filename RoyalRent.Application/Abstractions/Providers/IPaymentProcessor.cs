
using RoyalRent.Domain.Payments.Entities;

namespace RoyalRent.Application.Abstractions.Providers;

public interface IPaymentProcessor
{
    public Task<IntentPaymentsInformation> CreatePaymentAndRetrieveClientSecret(
        decimal amount,
        Guid rentId,
        Guid carId,
        Guid userId
    );
}
