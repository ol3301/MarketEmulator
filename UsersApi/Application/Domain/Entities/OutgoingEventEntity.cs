using UsersApi.Application.Domain.Events;

namespace UsersApi.Application.Domain.Entities;

public class OutgoingEventEntity
{
    public int OutgoingEventId { get; set; }
    public IntegrationEventType EventType { get; set; }
    public string EventData { get; set; }
    public DateTime CreatedAt { get; set; }
}