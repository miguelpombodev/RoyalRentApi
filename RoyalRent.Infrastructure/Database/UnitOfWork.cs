using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoyalRent.Application.Repositories;

namespace RoyalRent.Infrastructure.Database;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApiDbContext _dbContext;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(ApiDbContext dbContext, ILogger<UnitOfWork> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogCritical(
                e,
                "[CRITICAL] A concurrency violation is encountered when trying to saving any data in context - Message: {ErrorMessage} Trace: {ErrorTrace}",
                e.Message, e.StackTrace);
        }
        catch (DbUpdateException e)
        {
            _logger.LogCritical(
                e,
                "[CRITICAL] There was an error when trying to saving any data in context - Message: {ErrorMessage} Trace: {ErrorTrace}",
                e.Message, e.StackTrace);
        }
    }
}
