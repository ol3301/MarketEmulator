using ProjectsApi.Application.Domain.Interfaces;

namespace ProjectsApi.Application.EventHandlers;

public class EventHandlerFactory(
    UserCreatedEventHandler userCreatedEventHandler,
    SubscriptionUpdatedEventHandler subscriptionUpdatedEventHandler
    ): IEventHandlerFactory
{
    public UserCreatedEventHandler UserCreatedEventHandlerFactory { get; } = userCreatedEventHandler;
    public SubscriptionUpdatedEventHandler SubscriptionUpdatedEventHandlerFactory { get; } = subscriptionUpdatedEventHandler;
}