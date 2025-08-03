using ProjectsApi.Application.Domain.Events;
using ProjectsApi.Application.Infrastructure.Nats;
using ProjectsApi.Application.Services;

namespace ProjectsApi.Application.Background;

public class IntegrationEventsSyncService(
    NatsEventsConsumer consumer, 
    ILogger<IntegrationEventsSyncService> logger,
    UserEventSyncService userEventSyncService
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (var msg in consumer.ConsumeAsync<UserUpdatedEvent>("integration-events", cancellationToken))
        {
            logger.LogInformation("Received event: {Message}", msg.Data);

            await userEventSyncService.HandleUserUpdatedEventAsync(msg.Data!);
            
            await msg.AckAsync();
        }
    }
}