using GroupChat.Core;
using GroupChat.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupChat.Services;

public class UserService : IUserService
{
    private readonly ChatDbContext _dbContext;

    public UserService(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetUserById(int id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User> CreateUser(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task UpdateUser(int id, User user)
    {
        if (id != user.Id)
        {
            throw new ArgumentException("Id mismatch");
        }

        var existingUser = await _dbContext.Users.FindAsync(id);

        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }

        existingUser.Name = user.Name;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteUser(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }

        _dbContext.Users.Remove(user);

        await _dbContext.SaveChangesAsync();
    }
}
