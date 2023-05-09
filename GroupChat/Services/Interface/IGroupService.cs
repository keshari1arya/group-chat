using System.Collections.Generic;
using System.Threading.Tasks;
using GroupChat.Models;

namespace GroupChat.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetAllGroups();
        Task<Group> GetGroupById(int id);
        IQueryable<User> GetUsersByGroupId(int groupId);
        Task CreateGroup(Group group);
        Task UpdateGroup(Group group);
        Task DeleteGroup(int id);
        Task AddUserToGroup(int groupId, int userId);
        Task<bool> RemoveUserFromGroup(int groupId, int userId);
        IQueryable<GroupMessage> GetGroupMessages(int groupId);
        Task SendMessageToGroup(int groupId, int senderId, string message);
        Task<bool> ToggleLikeMessage(int userId, int messageId);
    }
}
