using System.Text.Json;
using UsersApi.Application.Domain.Entities;
using UsersApi.Application.Domain.Events;
using UsersApi.Application.Domain.Interfaces;
using UsersApi.Application.Infrastructure.Postgres;

namespace UsersApi.Application.Domain.Services;

public class EventPublisherService(UsersDbContext context) : IEventPublisherService
{
    public async Task PublishEventAsync<T>(T data, IntegrationEventType eventType) where T : IIntegrationEventDto
    {
        await context.OutgoingEvents.AddAsync(new OutgoingEventEntity
        {
            EventType = eventType,
            EventData = JsonSerializer.Serialize(data)
        });

        await context.SaveChangesAsync();
    }
}