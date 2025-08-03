using Core;
using NATS.Net;
using UsersApi.Application.Domain.Events;
using UsersApi.Application.Infrastructure.Nats;
using Xunit;

namespace UsersTests;

public class NatsEventPublisherTests
{
    private readonly NatsEventPublisher _publisher;
    
    public NatsEventPublisherTests()
    {
        _publisher = new NatsEventPublisher(new NatsClient(), "integration-events");
    }
    
    //[Fact]
    public async Task PublishOneMessageTest()
    {
        await _publisher.PublishEventAsync(new UserUpdatedEvent(2, 
            new SubscriptionEventDto(SubscriptionType.Free, 
                DateTime.Today, 
                DateTime.Today.AddDays(30))), 
            IntegrationEventType.UserUpdated);
    }
    
    //[Fact]
    public async Task PublishManyMessageTest()
    {
        foreach (var _ in Enumerable.Range(0, 10))
        {
            await _publisher.PublishEventAsync(new UserUpdatedEvent(1, 
                    new SubscriptionEventDto(SubscriptionType.Super, 
                        DateTime.Today, 
                        DateTime.Today.AddDays(30))), 
                IntegrationEventType.UserUpdated);
        }
    }
}