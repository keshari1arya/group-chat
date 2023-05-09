namespace GroupChat.Models;

public class GroupMessage : BaseEntity
{
    public int GroupId { get; set; }
    public int SenderId { get; set; }
    public string Text { get; set; }
    public DateTime SentAt { get; set; }
    public virtual Group Group { get; set; }
    public virtual User Sender { get; set; }
    public virtual ICollection<MessageLike> MessageLikes { get; set; }
}
