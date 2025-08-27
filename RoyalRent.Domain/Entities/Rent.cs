namespace RoyalRent.Domain.Entities;

public class Rent : BaseEntity
{
    public Rent(Guid userId, Guid carId, DateTime rentStartsAt, DateTime rentFinishesAt, bool isPaid,
        DateTime? paymentAt)
    {
        UserId = userId;
        CarId = carId;
        RentStartsAt = rentStartsAt;
        RentFinishesAt = rentFinishesAt;
        IsPaid = isPaid;
        PaymentAt = paymentAt;
    }

    public Guid UserId { get; set; }
    public Guid CarId { get; set; }
    public DateTime RentStartsAt { get; set; }
    public DateTime RentFinishesAt { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaymentAt { get; set; }
    public User User { get; set; } = null!;
    public Car Car { get; set; } = null!;
}
