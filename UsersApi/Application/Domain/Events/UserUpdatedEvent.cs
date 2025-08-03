using Core;

namespace UsersApi.Application.Domain.Events;

public record UserUpdatedEvent(int UserId, SubscriptionEventDto? Subscription) : IBaseEvent;
public record SubscriptionEventDto(SubscriptionType? SubscriptionType, DateTime StartDate, DateTime EndDate);