using System.ComponentModel.DataAnnotations;

namespace UsersApi.Application.Dtos;

public class UserCreateDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}