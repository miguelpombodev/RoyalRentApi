using RoyalRent.Domain.Cars.Entities;
using RoyalRent.Domain.Common.Entities;
using RoyalRent.Domain.Users.Entities;

namespace RoyalRent.Domain.Rents.Entities;

public class Rent : BaseEntity
{
    private const double RentFeeValue = 0.12;

    public Rent(Guid userId, Guid carId, decimal amount, DateTime rentStartsAt, DateTime rentFinishesAt,
        DateTime? paymentAt = null, bool isPaid = false)
    {
        UserId = userId;
        CarId = carId;
        RentStartsAt = rentStartsAt;
        RentFinishesAt = rentFinishesAt;
        IsPaid = isPaid;
        PaymentAt = paymentAt;
        RentTotalDays = GetRentDaysTotal();
        Amount = amount;
        AmountWithFee = CalculateRentTotalAmountWithFee();
    }

    public Guid UserId { get; set; }
    public Guid CarId { get; set; }
    public decimal Amount { get; set; }
    public decimal AmountWithFee { get; set; }
    public int RentTotalDays { get; set; }
    public DateTime RentStartsAt { get; set; }
    public DateTime RentFinishesAt { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaymentAt { get; set; }
    public User User { get; set; } = null!;
    public Car Car { get; set; } = null!;

    private decimal CalculateRentTotalAmountWithFee()
    {
        Amount *= RentTotalDays;

        return Math.Round(Amount * (1 + new decimal(RentFeeValue)), 2, MidpointRounding.AwayFromZero);
    }

    public void MarkRentAsPaid()
    {
        if (IsPaid && PaymentAt.HasValue) throw new InvalidOperationException("Rent cannot be mark as paid again!");

        if (DateTime.UtcNow > RentFinishesAt)
            throw new InvalidOperationException("It is not possible to pay a rent that the deadline has passed");

        IsPaid = true;
        PaymentAt = DateTime.UtcNow;
    }

    private TimeSpan GetRentDurationSpan()
    {
        return RentFinishesAt - RentStartsAt;
    }

    public int GetRentDaysTotal()
    {
        var span = GetRentDurationSpan();
        return (int)Math.Ceiling(span.TotalDays);
    }
}
