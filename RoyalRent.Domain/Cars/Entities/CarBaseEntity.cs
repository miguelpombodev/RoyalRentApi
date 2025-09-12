using RoyalRent.Domain.Common.Entities;

namespace RoyalRent.Domain.Entities;

public abstract class CarBaseEntity : BaseEntity
{
    protected CarBaseEntity(string name)
    {
        Name = name;
    }
    public string Name { get; set; }
}
