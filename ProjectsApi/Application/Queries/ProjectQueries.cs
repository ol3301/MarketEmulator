using Core;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProjectsApi.Application.Domain.Entities;
using ProjectsApi.Application.Dtos;
using ProjectsApi.Application.Infrastructure.MongoDb;

namespace ProjectsApi.Application.Queries;

public class ProjectQueries(MongoDbContext context)
{
    public async Task<MostUsedIndicatorsResponseDto> GetMostUsedIndecatorsAsync(int subscriptionType)
    {
        if (!Enum.IsDefined(typeof(SubscriptionType), subscriptionType))
        {
            throw new DomainException($"Invalid subscription type. {subscriptionType}");
        }
        
        var subscriptionId = (SubscriptionType)subscriptionType;

        var entries = await context.Projects
            .AsQueryable()
            .Lookup<ProjectEntity, UserEntity, UserEntity>(context.Users,
                (project, users) => users.Where(x => x.UserId == project.UserId 
                                                     && x.Subscription != null 
                                                     && x.Subscription.EndDate >= DateTime.UtcNow 
                                                     && x.Subscription.SubscriptionType == subscriptionId))
            .Where(x => x.Results.Length > 0 && x.Local.Charts!.Any() == true)
            .SelectMany(p => p.Local.Charts!.SelectMany(i => i.Indicators!))
            .GroupBy(x => x.Name)
            .Select(x => new MostUsedIndicatorEntryDto
            {
                Name = x.Key,
                Used = x.Count()
            })
            .OrderByDescending(x => x.Used)
            .Take(3)
            .ToListAsync();

        return new MostUsedIndicatorsResponseDto { Indicators = entries };
    }
}