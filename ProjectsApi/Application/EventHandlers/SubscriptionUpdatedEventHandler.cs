using Core;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProjectsApi.Application.Domain.Entities;
using ProjectsApi.Application.Domain.Events;
using ProjectsApi.Application.Infrastructure.MongoDb;

namespace ProjectsApi.Application.EventHandlers;

public class SubscriptionUpdatedEventHandler(MongoDbContext context)
{
    public async Task HandleAsync(SubscriptionUpdatedEventDto dto)
    {
        var entity = await context.Users.AsQueryable().FirstOrDefaultAsync(x => x.UserId == dto.UserId);
        if (entity == null)
        {
            throw new DomainException($"User with id {dto.UserId} does not exist.");
        }

        var update = Builders<UserEntity>.Update
            .Set(x => x.Subscription, new UserEntity.SubscriptionEntity
            {
                SubscriptionType = dto.SubscriptionTypeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            });

        await context.Users.UpdateOneAsync(x => x.UserId == dto.UserId, update, new UpdateOptions());
    }
}