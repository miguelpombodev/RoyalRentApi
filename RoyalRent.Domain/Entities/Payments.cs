namespace RoyalRent.Domain.Entities;

public class Payment : BaseEntity
{
    public Payment(Guid userId, Guid paymentStatusId)
    {
        UserId = userId;
        PaymentStatusId = paymentStatusId;
    }

    public Guid UserId { get; set; }
    public Guid PaymentStatusId { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = null!;
}
