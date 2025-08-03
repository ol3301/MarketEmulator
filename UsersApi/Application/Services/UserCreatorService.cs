using UsersApi.Application.Domain.Entities;
using UsersApi.Application.Domain.Events;
using UsersApi.Application.Domain.Interfaces;
using UsersApi.Application.Dtos;
using UsersApi.Application.Infrastructure.Postgres;

namespace UsersApi.Application.Services;

public class UserCreatorAppService(UsersDbContext context, IIntegrationEventPublisher publisher)
{
    public async Task<int> CreateAsync(UserCreateDto model)
    {
        var entity = new UserEntity
        {
            Name = model.Name,
            Email = model.Email,
        };

        await context.Users.AddAsync(entity);
        await context.SaveChangesAsync();

        await publisher.PublishEventAsync(new UserUpdatedEvent(entity.Id, null), IntegrationEventType.UserUpdated);

        return entity.Id;
    }
}