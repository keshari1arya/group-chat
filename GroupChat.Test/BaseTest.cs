using System;
using System.Reflection.Emit;
using AutoMapper;
using GroupChat.Config;
using GroupChat.Core;
using GroupChat.Models;
using GroupChat.Services;
using Microsoft.EntityFrameworkCore;

namespace GroupChat.Test
{
    public class BaseTest:IDisposable
    {
        internal readonly ChatDbContext _dbContext;
        internal readonly IMapper _mapper;

        public BaseTest()
        {
            var options = new DbContextOptionsBuilder<ChatDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            _dbContext = new ChatDbContext(options);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = config.CreateMapper();

        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        internal void Init()
        {
            if (_dbContext?.Users?.Any() != true)
            {
                int numUsers = 10;

                // Create users
                var users = new List<User>();
                for (int i = 1; i <= numUsers; i++)
                {
                    var user = new User
                    {
                        Id = i,
                        Name = $"User {i}",
                        Email = $"user{i}@mail.com",
                        Password = $"password{i}",
                        Username = $"user{i}"
                    };
                    users.Add(user);
                }

                _dbContext.Users.AddRange(users);
                _dbContext.SaveChanges();

                // Create 5 groups
                var groups = new List<Group>();
                for (int i = 1; i <= 5; i++)
                {
                    var group = new Group
                    {
                        Id = i,
                        Name = $"Group {i}",
                        Description = $"Description {i}",
                        CreatorId = users[new Random().Next(users.Count)].Id
                    };
                    groups.Add(group);
                }


                var rand = new Random();
                foreach (var user in users)
                {
                    var group = groups[rand.Next(groups.Count)];
                }
                _dbContext.Groups.AddRange(groups);
                _dbContext.SaveChanges();

                // Create group messages
                foreach (var group in groups)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        var message = new GroupMessage
                        {
                            Id = i * 10 + group.Id,
                            Text = $"Message {i} from {group.Name}",
                            GroupId = group.Id,
                            SenderId = users[rand.Next(users.Count)].Id
                        };
                        _dbContext.GroupMessages.Add(message);
                        // Create message likes
                        foreach (var liker in users)
                        {
                            if (rand.Next(2) == 0)
                            {
                                var like = new MessageLike
                                {
                                    Id = message.Id * 10 + liker.Id,
                                    GroupMessageId = message.Id,
                                    UserId = liker.Id
                                };
                                _dbContext.MessageLikes.Add(like);
                            }
                        }
                    }
                }

                _dbContext.SaveChanges();
            }
        }
    }
}