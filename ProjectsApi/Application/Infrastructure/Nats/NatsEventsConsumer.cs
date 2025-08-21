using System.Runtime.CompilerServices;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;

namespace ProjectsApi.Application.Infrastructure.Nats;

public class NatsEventsConsumer(NatsClient client, ILogger<NatsEventsConsumer> logger)
{
    public async IAsyncEnumerable<NatsJSMsg<T>> ConsumeAsync<T>(string streamName, [EnumeratorCancellation]CancellationToken cancellationToken)
    {
        var context = client.CreateJetStreamContext();
        await context.CreateOrUpdateStreamAsync(new StreamConfig
        {
            Name = streamName,
            Retention = StreamConfigRetention.Workqueue
        } ,cancellationToken);
        var consumer = await context.CreateOrUpdateConsumerAsync(streamName, new ConsumerConfig(streamName), cancellationToken);
        logger.LogInformation("Nats consumer created: {ConsumerName}", consumer.Info.Name);
        
        await foreach(var msg in consumer.ConsumeAsync<T>().WithCancellation(cancellationToken))
        {
            yield return msg;
        }

        logger.LogInformation("Exiting Nats consumer");
    }
}