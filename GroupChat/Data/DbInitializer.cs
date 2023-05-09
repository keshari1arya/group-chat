using GroupChat.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GroupChat.Core
{
    public static class DbInitializer
    {
        public static void Initialize(ModelBuilder modelBuilder)
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
                modelBuilder.Entity<User>().HasData(user);
                users.Add(user);
            }

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
                modelBuilder.Entity<Group>().HasData(group);
                groups.Add(group);
            }


            var rand = new Random();
            foreach (var user in users)
            {
                var group = groups[rand.Next(groups.Count)];
                modelBuilder.Entity<GroupUserXREF>().HasData(new GroupUserXREF { GroupId = group.Id, UserId = user.Id });
            }

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
                    modelBuilder.Entity<GroupMessage>().HasData(message);

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
                            modelBuilder.Entity<MessageLike>().HasData(like);
                        }
                    }
                }
            }
        }
    }
}
