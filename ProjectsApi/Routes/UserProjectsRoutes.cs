using ProjectsApi.Application.Dtos;
using ProjectsApi.Application.Services;

namespace ProjectsApi.Routes;

public static class UserProjectsRoutes
{
    public static IEndpointRouteBuilder MapUserProjectsRoutes(this IEndpointRouteBuilder builder)
    {
        builder.MapPut("/api/users/{userId:int}/projects", async (int userId, ProjectCreateDto model, ProjectCreatorService creatorService) =>
        {
            await creatorService.CreateAsync(userId, model);
            return Results.Ok();
        });
        
        return builder;
    }
}