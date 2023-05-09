namespace GroupChat.Dto;

public class UserRequest
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public class GroupRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}