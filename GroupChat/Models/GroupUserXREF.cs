namespace GroupChat.Models;

public class GroupUserXREF
{
    public int GroupId { get; set; }
    public Group Group { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
