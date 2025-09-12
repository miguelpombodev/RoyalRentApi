using RoyalRent.Domain.Common.Entities;

namespace RoyalRent.Domain.Payments.Entities;

public class PaymentStatus : BaseEntity
{
    public PaymentStatus(string name, string statusColor = "#00000")
    {
        Name = name;
        StatusColor = statusColor;
    }

    public string Name { get; set; }
    public string StatusColor { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
