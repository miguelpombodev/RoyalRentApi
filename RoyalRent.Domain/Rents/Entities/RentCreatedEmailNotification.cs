using RoyalRent.Domain.Common.Entities;

namespace RoyalRent.Domain.Rents.Entities;

public class RentCreatedEmailNotification : ServiceBusNotification
{
    public RentCreatedEmailNotification(string to) : base("sub-email-sender",
        "sub-email-sender-exchange", "sub-email", "<h1>Created Rent!</h1>")
    {
        To = to;
        Subject = "Created Rent!";
    }

    public string To { get; set; }
    public string Subject { get; set; }
}
