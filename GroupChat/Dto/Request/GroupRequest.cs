using System.ComponentModel.DataAnnotations;

namespace GroupChat.Dto;

public class GroupRequest
{
    public int Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(2000)]
    public string Description { get; set; }
}