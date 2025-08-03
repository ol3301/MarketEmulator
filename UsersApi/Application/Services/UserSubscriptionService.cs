using Core;
using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Domain.Entities;
using UsersApi.Application.Domain.Events;
using UsersApi.Application.Domain.Interfaces;
using UsersApi.Application.Dtos;
using UsersApi.Application.Infrastructure.Postgres;

namespace UsersApi.Application.Services;

public class UserSubscriptionService(UsersDbContext context, IIntegrationEventPublisher publisher)
{
    public async Task SubscribeAsync(int id, UserSubscribeDto model)
    {
        var user = await context.Users
            .Include(x => x.Subscription)
            .FirstOrDefaultAsync(x => x.Id == id);
            
        if (user == null)
        {
            throw new DomainException($"User not found. {id}");
        }

        if (user.Subscription != null && user.Subscription.EndDate.Date >= DateTime.UtcNow.Date)
        {
            throw new DomainException("User already has an active subscription.");
        }
        
        user.Subscription = new SubscriptionEntity
        {
            SubscriptionTypeId = model.SubscriptionTypeId,
            StartDate = model.StartDate.ToUniversalTime(),
            EndDate = model.EndDate.ToUniversalTime()
        };

        await context.SaveChangesAsync();
        await publisher.PublishEventAsync(new UserUpdatedEvent(user.Id, 
            new SubscriptionEventDto(model.SubscriptionTypeId, 
                model.StartDate,
                model.EndDate)), 
            IntegrationEventType.UserUpdated);
    }

    public async Task UpdateSubscriptionAsync(int id, UserUpdateSubscriptionDto model)
    {
        var user = await context.Users
            .Include(x => x.Subscription)
            .FirstOrDefaultAsync(x => x.Id == id);
            
        if (user == null)
        {
            throw new DomainException($"User not found. {id}");
        }

        if (user.Subscription == null)
        {
            throw new DomainException("User does not have an subscription to update.");
        }
        
        user.Subscription.SubscriptionTypeId = model.SubscriptionTypeId;
        user.Subscription.StartDate = model.StartDate.ToUniversalTime();
        user.Subscription.EndDate = model.EndDate.ToUniversalTime();
        
        await context.SaveChangesAsync();
        await publisher.PublishEventAsync(new UserUpdatedEvent(user.Id, 
                new SubscriptionEventDto(model.SubscriptionTypeId, 
                    model.StartDate,
                    model.EndDate)), 
            IntegrationEventType.UserUpdated);
    }
}