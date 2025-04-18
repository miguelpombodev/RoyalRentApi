using RoyalRent.Application.Repositories;

namespace RoyalRent.Infrastructure.Database;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApiDbContext _dbContext;

    public UnitOfWork(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
