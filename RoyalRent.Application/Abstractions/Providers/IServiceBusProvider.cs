using RoyalRent.Domain.Common.Entities;

namespace RoyalRent.Application.Abstractions.Providers;

public interface IServiceBusProvider
{
    Task<ServiceBusSendMessageResult> SendMessage(ServiceBusNotification notification);
}
