using UsersApi.Application.Domain.Events;

namespace UsersApi.Application.Domain.Interfaces;

public interface IIntegrationEventPublisher
{
    Task PublishEventAsync<T>(T data, IntegrationEventType eventType) where T : IBaseEvent;
}