using System.ComponentModel.DataAnnotations;

namespace GroupChat.Models;

public class Group : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CreatorId { get; set; }
    public ICollection<GroupUserXREF> GroupUserXREF { get; set; }
    public ICollection<GroupMessage> GroupMessages { get; set; }
}
public class BaseEntity
{
    [Key]
    // Not using this because of seed data
    // For production please enable this
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
}