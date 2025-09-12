namespace RoyalRent.Domain.Common.Entities;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedOn = DateTime.Now.ToUniversalTime();
        UpdatedOn = DateTime.Now.ToUniversalTime();
    }

    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}
