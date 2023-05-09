namespace GroupChat.Models;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public ICollection<GroupUserXREF> GroupUserXREF { get; set; }
    public ICollection<MessageLike> MessageLikes { get; set; }
    public ICollection<GroupMessage> GroupMessages { get; set; }
}
