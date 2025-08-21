using UsersApi.Application.Domain.Events;

namespace UsersApi.Application.Domain.Interfaces;

public interface IEventPublisherService
{
    Task PublishEventAsync<T>(T data, IntegrationEventType eventType) where T : IIntegrationEventDto;
}