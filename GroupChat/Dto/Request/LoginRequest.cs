using System.ComponentModel.DataAnnotations;

namespace GroupChat.Dto;

public class LoginRequest
{
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Username { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(1000)]
    public string Password { get; set; }
}