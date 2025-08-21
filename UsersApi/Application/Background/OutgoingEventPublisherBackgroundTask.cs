using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Domain.Interfaces;
using UsersApi.Application.Infrastructure.Postgres;

namespace UsersApi.Application.Background;

public class OutgoingEventPublisherBackgroundTask(
    IServiceScopeFactory factory,
    IQueuePublisher publisher
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var context = factory.CreateScope().ServiceProvider.GetRequiredService<UsersDbContext>();
        
        while (!cancellationToken.IsCancellationRequested)
        {
            var events = await context.OutgoingEvents
                .OrderBy(x => x.CreatedAt)
                .Take(1000)
                .ToListAsync();
            
            foreach (var outgoingEvent in events)
            {
                await publisher.PublishAsync(outgoingEvent.EventData, outgoingEvent.EventType.ToString());
            }
            
            context.OutgoingEvents.RemoveRange(events);
            await context.SaveChangesAsync();

            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
    }
}