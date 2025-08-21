using ProjectsApi.Application.EventHandlers;

namespace ProjectsApi.Application.Domain.Interfaces;

public interface IEventHandlerFactory
{
    public UserCreatedEventHandler UserCreatedEventHandlerFactory { get; }
    public SubscriptionUpdatedEventHandler SubscriptionUpdatedEventHandlerFactory { get; } 
}