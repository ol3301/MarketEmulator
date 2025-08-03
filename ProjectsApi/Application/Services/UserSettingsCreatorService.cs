using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProjectsApi.Application.Domain.Entities;
using ProjectsApi.Application.Dtos;
using ProjectsApi.Application.Infrastructure.MongoDb;

namespace ProjectsApi.Application.Services;

public class UserSettingsCreatorService(MongoDbContext context)
{
    public async Task CreateOrUpdateAsync(int userId, UserSettingsUpdateDto model)
    {
        var entity = await context.UserSettings.AsQueryable().FirstOrDefaultAsync(x => x.UserId == userId);
        await context.UserSettings.ReplaceOneAsync(x => x.UserId == userId, new UserSettingsEntity
        {
            Id = entity?.Id == null ? ObjectId.GenerateNewId() : entity.Id,
            UserId = userId,
            Language = model.Language,
            Theme = model.Theme
        }, new ReplaceOptions{IsUpsert = true});
    }
}