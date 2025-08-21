using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;
using UsersApi.Application.Domain.Interfaces;

namespace UsersApi.Application.Infrastructure.Nats;

public class NatsQueuePublisher(NatsClient client, string subjectName) : IQueuePublisher
{
    private Lazy<Task<INatsJSContext>> _contextFactory = new(async () =>
    {
        var context = client.CreateJetStreamContext();

        await context.CreateOrUpdateStreamAsync(new StreamConfig
        {
            Name = subjectName,
            Retention = StreamConfigRetention.Workqueue
        });

        return context;
    });

    public async Task PublishAsync(string data, string eventType)
    {
        var context = await _contextFactory.Value;
        
        await context.PublishAsync(subjectName, data, headers: new NatsHeaders
        { 
            { "eventType", eventType },
        });
    }
}