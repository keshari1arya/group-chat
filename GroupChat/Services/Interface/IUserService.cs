using GroupChat.Dto;
using GroupChat.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupChat.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserById(int id);
    Task<User> CreateUser(UserRequest user);
    Task UpdateUser(UserRequest user);
    Task DeleteUser(int id);
    Task<User> Authenticate(string username, string password);
}
