namespace ProjectsApi.Application.Dtos;

public class MostUsedIndicatorsResponseDto
{
    public IEnumerable<MostUsedIndicatorEntryDto> Indicators { get; set; }
}

public class MostUsedIndicatorEntryDto
{
    public string Name { get; set; }
    public int Used { get; set; }
}