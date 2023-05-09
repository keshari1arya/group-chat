using AutoMapper;
using GroupChat.Core;
using GroupChat.Dto;
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
    private readonly IMapper _mapper;

    public UserService(ChatDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async Task<User> GetUserById(int id)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _dbContext.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User> CreateUser(UserRequest user)
    {
        var userEntity = _mapper.Map<User>(user);
        _dbContext.Users.Add(userEntity);
        await _dbContext.SaveChangesAsync();
        return userEntity;
    }

    public async Task UpdateUser(UserRequest user)
    {
        _dbContext.Users.Update(_mapper.Map<User>(user));
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteUser(int id)
    {
        var user = await _dbContext.Users
        .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }

        _dbContext.Users.Remove(user);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<User> Authenticate(string username, string password)
    {
        // For simplicity we are saving the user password in the database
        // we can create a a method to get a oneway password hash and save.

        var user = await _dbContext.Users.AsNoTracking()
        .SingleOrDefaultAsync(x => x.Username == username && x.Password == password);

        if (user == null)
            return null;

        // Authentication successful
        return user;
    }
}
