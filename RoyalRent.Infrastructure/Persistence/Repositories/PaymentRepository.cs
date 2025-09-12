using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoyalRent.Domain.Abstractions;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Payments.Entities;
using RoyalRent.Domain.Payments.Interfaces;
using RoyalRent.Infrastructure.Database;

namespace RoyalRent.Infrastructure.Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ILogger<PaymentRepository> _logger;
    private readonly DbSet<Payment> _paymentsContext;
    private readonly DbSet<PaymentStatus> _paymentsStatusContext;

    public PaymentRepository(ILogger<PaymentRepository> logger, ApiDbContext context)
    {
        _logger = logger;
        _paymentsContext = context.Set<Payment>();
        _paymentsStatusContext = context.Set<PaymentStatus>();
    }

    public async Task<Payment> Create(Payment payment)
    {
        var paymentEntry = await _paymentsContext.AddAsync(payment);
        _logger.LogInformation(
            "A payment register {PaymentId} was created successfully for User id [{UserId}] at {PaymentCreatedAt}",
            payment.Id, payment.UserId, payment.CreatedOn);

        return paymentEntry.Entity;
    }

    public Task<Payment> UpdatePaymentStatus(Payment payment, Guid paymentStatus)
    {
        throw new NotImplementedException();
    }

    public async Task<PaymentStatus> GetByNameOrCreatePaymentStatus(string statusName)
    {
        var getPaymentResult =
            await _paymentsStatusContext.FirstOrDefaultAsync(status => status.Name.Equals(statusName));

        if (getPaymentResult is not null)
        {
            return getPaymentResult;
        }

        var createNewPaymentStatus = await _paymentsStatusContext.AddAsync(
            new PaymentStatus(statusName)
        );

        _logger.LogInformation("Create new PaymentStatus {PaymentStatusName}", statusName);

        return createNewPaymentStatus.Entity;
    }
}
