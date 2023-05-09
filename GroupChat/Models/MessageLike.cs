namespace GroupChat.Models;

public class MessageLike : BaseEntity
{
    public int GroupMessageId { get; set; }
    public int UserId { get; set; }
    public virtual GroupMessage GroupMessage { get; set; }
    public virtual User User { get; set; }
}
