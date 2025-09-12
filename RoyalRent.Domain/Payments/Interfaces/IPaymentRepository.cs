using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Payments.Entities;

namespace RoyalRent.Domain.Payments.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> Create(Payment payment);
    Task<Payment> UpdatePaymentStatus(Payment payment, Guid paymentStatus);
    Task<PaymentStatus> GetByNameOrCreatePaymentStatus(string statusName);
}
