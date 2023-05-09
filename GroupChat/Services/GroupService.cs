using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupChat.Core;
using GroupChat.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupChat.Services;

public class GroupService : IGroupService
{
    private readonly ChatDbContext _dbContext;

    public GroupService(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Group>> GetAllGroups()
    {
        return await Task.FromResult(_dbContext.Groups.ToList());
    }

    public async Task<Group> GetGroupById(int id)
    {
        return await _dbContext.Groups.FindAsync(id);
    }

    public async Task CreateGroup(Group group)
    {
        await _dbContext.Groups.AddAsync(group);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateGroup(Group group)
    {
        _dbContext.Groups.Update(group);
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

    public IQueryable<User> GetUsersByGroupId(int groupId)
    {
        return _dbContext.Users.Where(x => x.GroupUserXREF.Any(x => x.GroupId == groupId));
    }

    public async Task AddUserToGroup(int groupId, int userId)
    {
        Group? group = await CheckUserGroupValidity(groupId, userId);

        if (group.GroupUserXREF.Any(x => x.UserId == userId))
        {
            throw new InvalidOperationException($"User with id {userId} is already in group with id {groupId}");
        }

        group.GroupUserXREF.Add(new GroupUserXREF { GroupId = groupId, UserId = userId });
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

    public IQueryable<GroupMessage> GetGroupMessages(int groupId)
    {
        return _dbContext.GroupMessages
        .Where(x => x.GroupId == groupId)
        .OrderByDescending(x => x.SentAt);
    }

    public async Task SendMessageToGroup(int groupId, int senderId, string message)
    {
        var group = await _dbContext.Groups
            .Include(g => g.GroupUserXREF)
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
        var message = await _dbContext.GroupMessages.FindAsync(messageId);
        if (message == null)
        {
            throw new KeyNotFoundException($"Message with id {messageId} not found");
        }

        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {userId} not found");
        }

        var like = await _dbContext.MessageLikes.FirstOrDefaultAsync(x => x.GroupMessageId == messageId && x.UserId == userId);
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
