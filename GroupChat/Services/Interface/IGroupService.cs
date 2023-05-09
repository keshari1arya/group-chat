using System.Collections.Generic;
using System.Threading.Tasks;
using GroupChat.Models;

namespace GroupChat.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetAllGroups();
        Task<Group> GetGroupById(int id);
        Task CreateGroup(Group group);
        Task UpdateGroup(Group group);
        Task DeleteGroup(int id);
    }
}
