using GroupChat.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupChat.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserById(int id);
    Task<User> CreateUser(User user);
    Task UpdateUser(int id, User user);
    Task DeleteUser(int id);
    Task<User> Authenticate(string username, string password);
}
