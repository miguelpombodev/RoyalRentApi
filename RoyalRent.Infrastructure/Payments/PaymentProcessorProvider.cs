using Microsoft.Extensions.Logging;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Domain.Payments.Builders;
using RoyalRent.Domain.Payments.Entities;
using Stripe;

namespace RoyalRent.Infrastructure.Payments;

public class PaymentProcessorProvider : IPaymentProcessor
{
    private readonly PaymentIntentService _paymentIntentService = new();
    private readonly ILogger<PaymentProcessorProvider> _logger;

    public PaymentProcessorProvider(ILogger<PaymentProcessorProvider> logger)
    {
        _logger = logger;
    }

    public async Task<IntentPaymentsInformation> CreatePaymentAndRetrieveClientSecret(
        decimal amount,
        Guid rentId,
        Guid carId,
        Guid userId)
    {
        try
        {
            var request = new PaymentBuilder().SetAmount(amount).SetMetadata(rentId, carId, userId).Build();
            var options = StripePaymentMapper.CreateOptions(request);

            var intent = await _paymentIntentService.CreateAsync(options);

            _logger.LogInformation("Created for User {UserId} and Car {CarId} at {CreatedRentOn}", userId,
                carId, DateTime.UtcNow);


            var intentResults = new IntentPaymentsInformation().SetClientSecret(intent.ClientSecret).SetPaymentStatus(intent.Status)
                .Build();

            return intentResults;
        }
        catch (StripeException exc)
        {
            var errorCode = exc.StripeError.Code;

            switch (errorCode)
            {
                case StripeErrorsConstants.DeclinedCard:
                    _logger.LogError(exc, "User Car was declined for reason ");
                    break;
                case StripeErrorsConstants.BlockedCard:
                    _logger.LogError(exc, "User Card is blocked and could not finish transaction");
                    break;
                case StripeErrorsConstants.GenericCardError:
                    _logger.LogError(exc,
                        "Something went wrong when trying to complete transaction. Please check logs!");
                    break;
                default:
                    _logger.LogError(exc, "Unhandled Stripe error: {Code} - {Message}", errorCode, exc.Message);
                    break;
            }

            throw;
        }
    }
}
