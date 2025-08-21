using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProjectsApi.Application.Domain.Entities;
using ProjectsApi.Application.Domain.Events;
using ProjectsApi.Application.Infrastructure.MongoDb;

namespace ProjectsApi.Application.EventHandlers;

public class UserCreatedEventHandler(MongoDbContext context)
{
    public async Task HandleAsync(UserCreatedEventDto dto)
    {
        var entity = await context.Users.AsQueryable().FirstOrDefaultAsync(x => x.UserId == dto.UserId);
        
        await context.Users.ReplaceOneAsync(u => u.UserId == dto.UserId, new UserEntity
        {
            Id = entity?.Id == null ? ObjectId.GenerateNewId() : entity.Id,
            UserId = dto.UserId
        }, new ReplaceOptions{IsUpsert = true});
    }
}