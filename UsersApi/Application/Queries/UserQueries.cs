using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Dtos;
using UsersApi.Application.Infrastructure.Postgres;

namespace UsersApi.Application.Queries;

public class UserQueries(UsersDbContext context)
{
    public async Task <UserResponseDto?> GetUserByIdAsync(int id)
    {
        return await context.Users
            .Where(u => u.Id == id)
            .Select(u => new UserResponseDto
            {
                Name = u.Name,
                Email = u.Email,
                SubscriptionId = u.SubscriptionId
            })
            .FirstOrDefaultAsync();
    }
}