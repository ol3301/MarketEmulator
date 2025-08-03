using ProjectsApi.Application.Dtos;
using ProjectsApi.Application.Queries;
using ProjectsApi.Application.Services;

namespace ProjectsApi.Routes;

public static class UserSettingsRoutes
{
    public static IEndpointRouteBuilder MapUserSettingsRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/{id:int}/settings/", async (int id, UserSettingsQueries queriesService) =>
        {
            return await queriesService.GetUserSettingsAsync(id) is { } dto
                ? Results.Ok(dto)
                : Results.NoContent();
        });
        
        app.MapPost("/api/users/{id:int}/settings", async (int id, UserSettingsUpdateDto model, UserSettingsCreatorService settingsCreatorService) =>
        {
            await settingsCreatorService.CreateOrUpdateAsync(id, model);
            return Results.Ok();
        });
        return app;
    }
}