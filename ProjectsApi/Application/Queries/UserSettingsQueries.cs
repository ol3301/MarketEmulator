using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProjectsApi.Application.Dtos;
using ProjectsApi.Application.Infrastructure.MongoDb;

namespace ProjectsApi.Application.Queries;

public class UserSettingsQueries(MongoDbContext context)
{
    public async Task<UserSettingsResponseDto> GetUserSettingsAsync(int userId)
    {
        return await context.UserSettings
            .AsQueryable()
            .Select(x => new UserSettingsResponseDto
            {
                UserId = x.UserId,
                Language = x.Language,
                Theme = x.Theme
            })
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
