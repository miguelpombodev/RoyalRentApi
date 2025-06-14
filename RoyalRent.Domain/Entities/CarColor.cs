namespace RoyalRent.Domain.Entities;

public class CarColor : CarBaseEntity
{
    public CarColor(string name) : base(name) { }

    public ICollection<Car>? Cars { get; set; } = new List<Car>();
}
