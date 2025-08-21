namespace UsersApi.Application.Domain.Interfaces;

public interface IQueuePublisher
{
    Task PublishAsync(string data, string eventType);
}