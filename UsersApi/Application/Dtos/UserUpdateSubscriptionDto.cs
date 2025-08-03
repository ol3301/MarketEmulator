using System.ComponentModel.DataAnnotations;
using Core;

namespace UsersApi.Application.Dtos;

public class UserUpdateSubscriptionDto
{
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public SubscriptionType SubscriptionTypeId { get; set; }
}