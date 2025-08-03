using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProjectsApi.Application.Domain.Entities;
using ProjectsApi.Application.Domain.Events;
using ProjectsApi.Application.Infrastructure.MongoDb;

namespace ProjectsApi.Application.Services;

public class UserEventSyncService(MongoDbContext context)
{
    public async Task HandleUserUpdatedEventAsync(UserUpdatedEvent e)
    {
        var entity = await context.Users.AsQueryable().FirstOrDefaultAsync(x => x.UserId == e.UserId);
        
        await context.Users.ReplaceOneAsync(u => u.UserId == e.UserId, new UserEntity
        {
            Id = entity?.Id == null ? ObjectId.GenerateNewId() : entity.Id,
            UserId = e.UserId,
            Subscription = e.Subscription != null ? new UserEntity.SubscriptionEntity()
            {
                SubscriptionType = e.Subscription.SubscriptionType,
                StartDate = e.Subscription.StartDate,
                EndDate = e.Subscription.EndDate
            } : null
        }, new ReplaceOptions{IsUpsert = true});
    }
}