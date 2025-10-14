using RabbitMQ.Client;

namespace RoyalRent.Infrastructure.Communication.Interfaces;

public interface IRabbitMqConnection
{
    Task<IChannel> CreateChannelAsync();

}
