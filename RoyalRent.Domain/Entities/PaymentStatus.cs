namespace RoyalRent.Domain.Entities;

public class PaymentStatus : BaseEntity
{
    public PaymentStatus(string name, string statusColor)
    {
        Name = name;
        StatusColor = statusColor;
    }

    public string Name { get; set; }
    public string StatusColor { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
