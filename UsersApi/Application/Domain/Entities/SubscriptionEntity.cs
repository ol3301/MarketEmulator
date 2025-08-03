using Core;

namespace UsersApi.Application.Domain.Entities;

public class SubscriptionEntity
{
    public int Id { get; set; }
    public SubscriptionType SubscriptionTypeId { get; set; }
    public SubscriptionTypeEntity SubscriptionType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
