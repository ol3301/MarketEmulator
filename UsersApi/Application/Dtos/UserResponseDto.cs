namespace UsersApi.Application.Dtos;

public class UserResponseDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int? SubscriptionId { get; set; }
}