using Core;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProjectsApi.Application.Domain.Entities;
using ProjectsApi.Application.Dtos;
using ProjectsApi.Application.Infrastructure.MongoDb;

namespace ProjectsApi.Application.Services;

public class ProjectCreatorService(MongoDbContext context)
{
    public async Task CreateAsync(int userId, ProjectCreateRequestDto model)
    {
        if(!await context.Users.AsQueryable().AnyAsync(x => x.UserId == userId))
        {
            throw new DomainException($"User with id {userId} does not exist.");
        }
        
        var entity = new ProjectEntity
        {
            Id = ObjectId.GenerateNewId(),
            UserId = userId,
            Name = model.Name,
            Charts = model.Charts?.Select(c => new ChartEntity
            {
                Symbol = c.Symbol,
                Timeframe = c.Timeframe,
                Indicators = c.Indicators?.Select(i => new IndicatorEntity
                {
                    Name = i.Name,
                    Parameters = i.Parameters
                }).ToList()
            }).ToList()
        };
        
        await context.Projects.InsertOneAsync(entity);
    }
}