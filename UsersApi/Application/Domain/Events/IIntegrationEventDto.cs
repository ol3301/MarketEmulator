using Core;

namespace UsersApi.Application.Domain.Events;

public interface IIntegrationEventDto
{
}

public enum IntegrationEventType
{ 
    UserCreated,
    SubscriptionUpdated
}

public record UserCreatedEventDto(int UserId) : IIntegrationEventDto;
public record SubscriptionUpdatedEventDto(int UserId, SubscriptionType SubscriptionTypeId, DateTime StartDate, DateTime EndDate) : IIntegrationEventDto;