namespace RoyalRent.Domain.Entities;

public abstract class BaseEntity
{
    public BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedOn = DateTime.Now.ToUniversalTime();
        UpdatedOn = DateTime.Now.ToUniversalTime();
    }
    
    public Guid Id { get; set; }
    private DateTime CreatedOn { get; set; }
    private DateTime UpdatedOn { get; set; }
}