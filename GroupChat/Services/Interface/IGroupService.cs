using System.Collections.Generic;
using System.Threading.Tasks;
using GroupChat.Dto;
using GroupChat.Models;

namespace GroupChat.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetAllGroups();
        Task<Group> GetGroupById(int id);
        IEnumerable<UserResponse> GetUsersByGroupId(int groupId);
        Task CreateGroup(GroupRequest group);
        Task UpdateGroup(GroupRequest group);
        Task DeleteGroup(int id);
        Task AddUserToGroup(int groupId, int userId);
        Task<bool> RemoveUserFromGroup(int groupId, int userId);
        IEnumerable<GroupedGroupMessageResponse> GetGroupMessagesGroupedByDate(int groupId, int currentUserId, int pageIndex, int pageSize);
        Task SendMessageToGroup(int groupId, int senderId, string message);
        Task<bool> ToggleLikeMessage(int userId, int messageId);
    }
}
