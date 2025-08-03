using MongoDB.Bson;

namespace ProjectsApi.Application.Domain.Entities;

public class UserSettingsEntity
{
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public int UserId { get; set; }
    public string Language { get; set; }
    public string Theme { get; set; }
}