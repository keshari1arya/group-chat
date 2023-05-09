using System;
using GroupChat.Models;

namespace GroupChat.Test
{
    public static class MockGroupData
    {
        public static Group GetAGroup()
        {
            return new Group
            {
                Name = Guid.NewGuid().ToString(),
                Description = "des",
                GroupUserXREF = new List<GroupUserXREF>
                {
                    new GroupUserXREF
                    {
                        User = new User
                        {
                            Name = Guid.NewGuid().ToString(),
                            Email = "",
                            Password = "",
                            Username = "",
                        }
                    }
                },
                GroupMessages = new List<GroupMessage>
                {
                    new GroupMessage{ Text = "some"},
                    new GroupMessage{ Text = "some 1"}
                },
            };
        }
    }
}

