using Core;
using MongoDB.Bson;

namespace ProjectsApi.Application.Domain.Entities;

public class UserEntity
{
    public ObjectId Id { get; set; }
    public int UserId { get; set; }
    public SubscriptionEntity? Subscription { get; set; }

    public class SubscriptionEntity
    {
        public SubscriptionType SubscriptionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}