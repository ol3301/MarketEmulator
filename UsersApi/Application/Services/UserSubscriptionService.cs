using Core;
using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Domain.Entities;
using UsersApi.Application.Domain.Events;
using UsersApi.Application.Domain.Interfaces;
using UsersApi.Application.Dtos;
using UsersApi.Application.Infrastructure.Postgres;

namespace UsersApi.Application.Services;

public class UserSubscriptionService(UsersDbContext context, IEventPublisherService publisher)
{
    public async Task SubscribeAsync(int id, UserSubscribeDto model)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
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

        await publisher.PublishEventAsync(new SubscriptionUpdatedEventDto(user.Id, 
            user.Subscription.SubscriptionTypeId, 
            user.Subscription.StartDate, 
            user.Subscription.EndDate), IntegrationEventType.SubscriptionUpdated);
        await transaction.CommitAsync();
    }

    public async Task UpdateSubscriptionAsync(int id, UserUpdateSubscriptionDto model)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

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

        await publisher.PublishEventAsync(new SubscriptionUpdatedEventDto(user.Id, 
            user.Subscription.SubscriptionTypeId, 
            user.Subscription.StartDate, 
            user.Subscription.EndDate), IntegrationEventType.SubscriptionUpdated);
        await transaction.CommitAsync();
    }
}