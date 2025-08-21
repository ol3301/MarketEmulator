using System.Text.Json;
using ProjectsApi.Application.Domain.Events;
using ProjectsApi.Application.EventHandlers;
using ProjectsApi.Application.Infrastructure.Nats;

namespace ProjectsApi.Application.Background;

public class IntegrationEventsSyncTask(
    NatsEventsConsumer consumer, 
    ILogger<IntegrationEventsSyncTask> logger,
    IServiceProvider provider
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (var msg in consumer.ConsumeAsync<string>("integration-events", cancellationToken))
        {
            logger.LogInformation("Received event: {Message}", msg.Data);

            var eventType = Enum.Parse<IntegrationEventType>(msg.Headers!["eventType"].Last()!);
            
            switch (eventType)
            {
                case IntegrationEventType.UserCreated:
                {
                    await provider.GetRequiredService<UserCreatedEventHandler>()
                        .HandleAsync(JsonSerializer.Deserialize<UserCreatedEventDto>(msg.Data!)!);
                    break;
                }
                case IntegrationEventType.SubscriptionUpdated:
                {
                    await provider.GetRequiredService<SubscriptionUpdatedEventHandler>()
                        .HandleAsync(JsonSerializer.Deserialize<SubscriptionUpdatedEventDto>(msg.Data!)!);
                    break;
                }
            }
 
            await msg.AckAsync();
        }
    }
}