namespace ProjectsApi.Application.Dtos;

public class UserSettingsResponseDto
{
    public int UserId { get; set; }
    public string Language { get; set; }
    public string Theme { get; set; }
}