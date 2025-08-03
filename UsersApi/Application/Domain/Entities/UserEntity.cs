namespace UsersApi.Application.Domain.Entities;

public class UserEntity
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public string Email { get; set; }
    public int? SubscriptionId { get; set; }
    public SubscriptionEntity? Subscription { get; set; }
}