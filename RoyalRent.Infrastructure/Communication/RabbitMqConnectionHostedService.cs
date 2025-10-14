using Microsoft.Extensions.Hosting;
using RoyalRent.Infrastructure.Communication.Interfaces;

namespace RoyalRent.Infrastructure.Communication;

public class RabbitMqConnectionHostedService : IHostedService
{
    private readonly IRabbitMqConnection _connection;

    public RabbitMqConnectionHostedService(IRabbitMqConnection connection)
    {
        _connection = connection;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_connection is RabbitMqPersistentConnection persistentConnection)
        {
            await persistentConnection.StartConnectionAsync(cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_connection is RabbitMqPersistentConnection persistentConnection)
        {
            await persistentConnection.StopConnectionAsync(cancellationToken);
        }
    }
}
