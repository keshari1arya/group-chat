namespace GroupChat.Dto;

public class GroupedGroupMessageResponse
{
    public DateTime Date { get; set; }
    public List<GroupMessageResponse> Messages { get; set; }
}

public class GroupMessageResponse
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string Text { get; set; }
    public DateTime SentAt { get; set; }
    public List<int> LikedByUsers { get; set; }
}