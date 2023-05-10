using System.ComponentModel.DataAnnotations;

namespace GroupChat.Dto;

public class UserRequest
{
    public int Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Username { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(1000)]
    public string Password { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(1000)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; }
}
