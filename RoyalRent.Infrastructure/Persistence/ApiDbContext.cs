using Microsoft.EntityFrameworkCore;

namespace RoyalRent.Infrastructure.Persistence;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Infrastructure.AssemblyReference.Assembly);
    }
}
