namespace ProjectsApi.Application.Dtos;

public class ProjectCreateDto
{
    public string Name { get; set; }
    public IEnumerable<ChartCreateDto>? Charts { get; set; } = new List<ChartCreateDto>();
}
public class ChartCreateDto
{
    public string Symbol { get; set; }
    public string Timeframe { get; set; }
    public IEnumerable<IndicatorCreateDto>? Indicators { get; set; } = new List<IndicatorCreateDto>();
}
public class IndicatorCreateDto
{
    public string Name { get; set; }
    public string Parameters { get; set; }
}