using System.ComponentModel.DataAnnotations;

namespace GroupChat.Dto;
public class MessageRequest
{
    [Required]
    public int SenderId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Message { get; set; }
}
