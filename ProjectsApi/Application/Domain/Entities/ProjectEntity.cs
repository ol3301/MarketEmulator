using MongoDB.Bson;

namespace ProjectsApi.Application.Domain.Entities;

public class ProjectEntity
{
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public int UserId { get; set; }
    public string Name { get; set; }
    public List<ChartEntity>? Charts { get; set; } = new();
}

public class ChartEntity
{
    public string Symbol { get; set; }
    public string Timeframe { get; set; }
    public List<IndicatorEntity>? Indicators { get; set; } = new();
}

public class IndicatorEntity
{
    public string Name { get; set; }
    public string Parameters { get; set; }
}