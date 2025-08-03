using UsersApi.Application.Dtos;
using UsersApi.Application.Queries;
using UsersApi.Application.Services;

namespace UsersApi.Routes;

public static class UsersRoutes
{
    public static IEndpointRouteBuilder MapUsersRoutes(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/users");
        group.MapGet("/{id:int}", async (int id, UserQueries queriesService) =>
        {
            return (await queriesService.GetUserByIdAsync(id)) is { } user
                ? Results.Ok(user)
                : Results.NoContent();
        });
        
        group.MapPost("/", async (UserCreateDto model, UserCreatorAppService creatorService) =>
        {
            var id = await creatorService.CreateAsync(model);
            
            return Results.Created($"/users/{id}", id);
        });

        group.MapPost("/{id:int}/subscription", async (int id, UserSubscribeDto model, UserSubscriptionService subscriptionService) =>
        {
            await subscriptionService.SubscribeAsync(id, model);

            return Results.Ok();
        });
        
        group.MapPut("/{id:int}/subscription", async (int id, UserUpdateSubscriptionDto model, UserSubscriptionService subscriptionService) =>
        {
            await subscriptionService.UpdateSubscriptionAsync(id, model);

            return Results.Ok();
        });

        return builder;
    }
}