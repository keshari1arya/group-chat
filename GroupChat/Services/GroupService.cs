using AutoMapper;
using GroupChat.Core;
using GroupChat.Dto;
using GroupChat.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupChat.Services;

public class GroupService : IGroupService
{
    private readonly ChatDbContext _dbContext;
    private readonly IMapper _mapper;
    public GroupService(ChatDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroupResponse>> GetAllGroups()
    {
        var groups =  await _dbContext.Groups.AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<GroupResponse>>(groups);
    }

    public async Task<GroupResponse> GetGroupById(int id)
    {
        var group = await _dbContext.Groups.AsNoTracking().FirstAsync(x => x.Id == id);
        return _mapper.Map<GroupResponse>(group);
    }

    public async Task CreateGroup(GroupRequest group)
    {
        await _dbContext.Groups.AddAsync(_mapper.Map<Group>(group));
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateGroup(GroupRequest group)
    {
        _dbContext.Groups.Update(_mapper.Map<Group>(group));
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteGroup(int id)
    {
        var group = await _dbContext.Groups.FindAsync(id);
        if (group != null)
        {
            _dbContext.Groups.Remove(group);
            await _dbContext.SaveChangesAsync();
        }
    }

    public IEnumerable<UserResponse> GetUsersByGroupId(int groupId)
    {
        var users = _dbContext.Users
        .Where(x => x.GroupUserXREF.Any(x => x.GroupId == groupId))
        .AsNoTracking();
        return _mapper.Map<IEnumerable<UserResponse>>(users);
    }

    public async Task AddUserToGroup(int groupId, int userId)
    {
        Group? group = await CheckUserGroupValidity(groupId, userId);

        if (group.GroupUserXREF.Any(x => x.UserId == userId))
        {
            throw new InvalidOperationException($"User with id {userId} is already in group with id {groupId}");
        }

        _dbContext.GroupUserXREF.Add(new GroupUserXREF { GroupId = groupId, UserId = userId });
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> RemoveUserFromGroup(int groupId, int userId)
    {
        var group = await CheckUserGroupValidity(groupId, userId);

        if (!group.GroupUserXREF.Any(x => x.UserId == userId))
        {
            throw new InvalidOperationException($"User with id {userId} is not in group with id {groupId}");
        }

        _dbContext.GroupUserXREF.Remove(group.GroupUserXREF.First());
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public IEnumerable<GroupedGroupMessageResponse> GetGroupMessagesGroupedByDate(int groupId, int currentUserId, int pageIndex, int pageSize)
    {
        // Check if the user is a member of the group
        var isMember = _dbContext.GroupUserXREF.Any(x => x.GroupId == groupId && x.UserId == currentUserId);

        if (!isMember)
        {
            throw new InvalidOperationException("User is not a member of the group or group does not exist");
        }
        var DbF = Microsoft.EntityFrameworkCore.EF.Functions;

        var messages = _dbContext.GroupMessages
        .Include(x => x.MessageLikes)
        .Where(x => x.GroupId == groupId)
        .OrderByDescending(x => x.SentAt)
        .Skip(pageIndex * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToList();

        var groupedMessages = messages.GroupBy(x => x.SentAt.Date)
        .Select(x => new GroupedGroupMessageResponse
        {
            Date = x.Key,
            Messages = _mapper.Map<List<GroupMessageResponse>>(x)
        });

        return groupedMessages;
    }



    public async Task SendMessageToGroup(int groupId, int senderId, string message)
    {
        var group = await _dbContext.Groups
            .Include(g => g.GroupUserXREF)
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == groupId);

        if (group == null)
        {
            throw new KeyNotFoundException($"Group with id {groupId} not found");
        }

        if (!group.GroupUserXREF.Any(gu => gu.UserId == senderId))
        {
            throw new InvalidOperationException($"User with id {senderId} is not a member of group with id {groupId}");
        }

        var groupMessage = new GroupMessage
        {
            GroupId = groupId,
            SenderId = senderId,
            Text = message,
            SentAt = DateTime.UtcNow
        };

        await _dbContext.GroupMessages.AddAsync(groupMessage);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ToggleLikeMessage(int userId, int messageId)
    {
        var message = await _dbContext.GroupMessages.AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == messageId);
        if (message == null)
        {
            throw new KeyNotFoundException($"Message with id {messageId} not found");
        }

        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {userId} not found");
        }

        var like = await _dbContext.MessageLikes
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.GroupMessageId == messageId && x.UserId == userId);
        if (like != null)
        {
            // User already liked the message, remove the like
            _dbContext.MessageLikes.Remove(like);
            await _dbContext.SaveChangesAsync();
            return false;
        }
        else
        {
            // User has not liked the message, add the like
            var newLike = new MessageLike { GroupMessageId = messageId, UserId = userId };
            _dbContext.MessageLikes.Add(newLike);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }



    private async Task<Group?> CheckUserGroupValidity(int groupId, int userId)
    {
        var group = _dbContext.Groups
        .Include(x => x.GroupUserXREF)
        .AsNoTracking()
        .FirstOrDefault(x => x.Id == groupId);
        if (group == null)
        {
            throw new KeyNotFoundException($"Group with id {groupId} not found");
        }

        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {userId} not found");
        }

        return group;
    }

}
