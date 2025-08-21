using System.Text.Json;
using Core;
using NATS.Net;
using UsersApi.Application.Domain.Events;
using UsersApi.Application.Infrastructure.Nats;
using Xunit;

namespace UsersTests;

public class NatsEventPublisherTests
{
    private readonly NatsQueuePublisher _publisher;
    
    public NatsEventPublisherTests()
    {
        _publisher = new NatsQueuePublisher(new NatsClient(), "integration-events");
    }
    
    //[Fact]
    public async Task PublishUserCreatedEventTest()
    {
        await _publisher.PublishAsync(JsonSerializer.Serialize(new UserCreatedEventDto(1)), $"{IntegrationEventType.UserCreated}");
    }
    
    //[Fact]
    public async Task PublishUsersCreatedEventTest()
    {
        foreach (var _ in Enumerable.Range(0, 10))
        {
            await _publisher.PublishAsync(JsonSerializer.Serialize(new UserCreatedEventDto(1)), $"{IntegrationEventType.UserCreated}");
        }
    }
    
    //[Fact]
    public async Task PublishSubscriptionUpdatedEventTest()
    {
        await _publisher.PublishAsync(JsonSerializer.Serialize(new SubscriptionUpdatedEventDto(1,
                SubscriptionType.Free,
                DateTime.Today,
                DateTime.Today.AddDays(30))),
            $"{IntegrationEventType.SubscriptionUpdated}");
    }
}