using MongoDB.Driver;
using ProjectsApi.Application.Domain.Entities;

namespace ProjectsApi.Application.Infrastructure.MongoDb;

public class MongoDbContext(IMongoClient client)
{
    public IMongoCollection<UserEntity> Users => 
        client.GetDatabase("Projects").GetCollection<UserEntity>("Users");
    public IMongoCollection<UserSettingsEntity> UserSettings => 
        client.GetDatabase("Projects").GetCollection<UserSettingsEntity>("UserSettings");
    public IMongoCollection<ProjectEntity> Projects => 
        client.GetDatabase("Projects").GetCollection<ProjectEntity>("Projects");
}