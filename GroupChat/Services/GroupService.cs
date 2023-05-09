using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupChat.Core;
using GroupChat.Models;

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
}
