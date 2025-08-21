using UsersApi.Application.Domain.Entities;
using UsersApi.Application.Domain.Events;
using UsersApi.Application.Domain.Interfaces;
using UsersApi.Application.Dtos;
using UsersApi.Application.Infrastructure.Postgres;

namespace UsersApi.Application.Services;

public class UserCreatorAppService(UsersDbContext context, IEventPublisherService publisher)
{
    public async Task<int> CreateAsync(UserCreateDto model)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        var entity = new UserEntity
        {
            Name = model.Name,
            Email = model.Email,
        };

        await context.Users.AddAsync(entity);
        await context.SaveChangesAsync();

        await publisher.PublishEventAsync(new UserCreatedEventDto(entity.Id), IntegrationEventType.UserCreated);
        await transaction.CommitAsync();

        return entity.Id;
    }
}