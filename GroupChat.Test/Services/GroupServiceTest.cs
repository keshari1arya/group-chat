using AutoMapper;
using GroupChat.Config;
using GroupChat.Core;
using GroupChat.Dto;
using GroupChat.Models;
using GroupChat.Services;
using Microsoft.EntityFrameworkCore;

namespace GroupChat.Test.Services;

[TestClass]
public class GroupServiceTest : BaseTest
{
    private readonly IGroupService _groupService;

    public GroupServiceTest()
    {
        _groupService = new GroupService(_dbContext, _mapper);
    }

    [TestInitialize]
    public void Initialize()
    {
        Init();
    }

    [TestMethod]
    public async Task GetAllGroups_ReturnsAllGroups()
    {
        // Arrange


        // Act
        var result = await _groupService.GetAllGroups();

        // Assert
        Assert.AreEqual(_dbContext.Groups.Count(), result.Count());
    }

    [TestMethod]
    public async Task GetGroupById_ReturnsCorrectGroup()
    {
        // Arrange       
        var groupId = _dbContext.Groups.First().Id;
        // Act
        var result = await _groupService.GetGroupById(groupId);

        // Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task CreateGroup_AddsNewGroupToDatabase()
    {
        // Arrange
        var group = new GroupRequest { Name = "New Group", Description = "Description" };

        // Act
        await _groupService.CreateGroup(group);

        // Assert
        var result = await _dbContext.Groups.FirstOrDefaultAsync(g => g.Name == "New Group");
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task UpdateGroup_UpdatesExistingGroup()
    {
        // Arrange
        var group = _dbContext.Groups.First();
        var updatedGroup = new GroupRequest { Id = group.Id, Name = "New Group Name", Description = group.Description };

        // Act
        _dbContext.Entry(group).State = EntityState.Detached;
        await _groupService.UpdateGroup(updatedGroup);

        // Assert
        var result = await _dbContext.Groups.FirstOrDefaultAsync(g => g.Id == group.Id);
        Assert.IsNotNull(result);
        Assert.AreEqual("New Group Name", result.Name);
    }

    [TestMethod]
    public async Task DeleteGroup_DeletesExistingGroup()
    {
        // Arrange
        var groupIdToDelete = 2;

        // Act
        await _groupService.DeleteGroup(groupIdToDelete);

        // Assert
        var deletedGroup = await _dbContext.Groups.FindAsync(groupIdToDelete);
        Assert.IsNull(deletedGroup);
    }

    [TestMethod]
    public async Task DeleteGroup_DeletesGroupWithMessages()
    {
        // Arrange

        var group = MockGroupData.GetAGroup();

        _dbContext.Groups.Add(group);
        _dbContext.SaveChanges();
        var createdGroup = _dbContext.Groups.First(x => x.Name == group.Name);

        // Act
        await _groupService.DeleteGroup(createdGroup.Id);

        // Assert
        var deletedGroup = await _dbContext.Groups.FindAsync(createdGroup.Id);
        Assert.IsNull(deletedGroup);

        var deletedMessages = await _dbContext.GroupMessages
            .Where(m => m.GroupId == createdGroup.Id)
            .ToListAsync();
        Assert.AreEqual(0, deletedMessages.Count);
    }

    [TestMethod]
    public async Task DeleteGroup_DeletesGroupWithLikes()
    {
        // Arrange
        var group = MockGroupData.GetAGroup();

        _dbContext.Groups.Add(group);
        _dbContext.SaveChanges();
        var createdGroup = _dbContext.Groups.First(x => x.Name == group.Name);

        _dbContext.MessageLikes.AddRange(new MessageLike { UserId = 1, });

        // Act
        await _groupService.DeleteGroup(createdGroup.Id);

        // Assert
        var deletedLikes = await _dbContext.MessageLikes
            .Where(l => l.GroupMessage.GroupId == createdGroup.Id)
            .ToListAsync();
        Assert.AreEqual(0, deletedLikes.Count);
    }

    [TestMethod]
    public void GetUsersByGroupId_ReturnsUsers_WhenGroupExists()
    {
        // Arrange
        var createGroup = MockGroupData.GetAGroup();

        _dbContext.Groups.Add(createGroup);
        _dbContext.SaveChanges();
        var group = _dbContext.Groups
            .Include(x => x.GroupUserXREF)
            .ThenInclude(x => x.User)
            .First(x => x.Name == createGroup.Name);


        // Act
        var result = _groupService.GetUsersByGroupId(group.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IEnumerable<UserResponse>));
        Assert.AreEqual(result.Count(), group.GroupUserXREF.Count());
        Assert.IsTrue(result.Any(x => x.Name == group.GroupUserXREF.First().User.Name));
    }

    [TestMethod]
    public async Task AddUserToGroup_ValidGroupAndUser_AddsUserToGroup()
    {
        // Arrange
        var user = _dbContext.Users
            .AsNoTracking()
            .FirstOrDefault();
        var createGroup = MockGroupData.GetAGroup();

        _dbContext.Groups.Add(createGroup);
        _dbContext.SaveChanges();

        var group = _dbContext.Groups
             .AsNoTracking()
             .FirstOrDefault(x => x.GroupUserXREF.Count > 0);


        // Act
        await _groupService.AddUserToGroup(group.Id, user.Id);

        // Assert
        var result = _dbContext.GroupUserXREF
            .FirstOrDefault(x => x.UserId == user.Id && x.GroupId == group.Id);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ShouldNotAddUserToGroup_IfUserAlreadyAMemberOfTheGroup()
    {
        // Arrange
        var user = _dbContext.Users
            .AsNoTracking()
            .FirstOrDefault();
        var createGroup = MockGroupData.GetAGroup();

        _dbContext.Groups.Add(createGroup);
        _dbContext.SaveChanges();

        var group = _dbContext.Groups
            .Include(x => x.GroupUserXREF)
             .AsNoTracking()
             .FirstOrDefault(x => x.GroupUserXREF.Count > 0);

        var userId = group.GroupUserXREF.First().UserId;

        // Act and Assert
        var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
        _groupService.AddUserToGroup(group.Id, userId));
        Assert.AreEqual($"User with id {userId} is already in group with id {group.Id}", exception.Message);
    }

    [TestMethod]
    public async Task Test_RemoveUserFromGroup_WhenGroupExistsAndUserBelongsToGroup()
    {
        // Arrange
        var createGroup = MockGroupData.GetAGroup();
        _dbContext.Groups.Add(createGroup);
        _dbContext.SaveChanges();

        var group = _dbContext.Groups
            .Include(x => x.GroupUserXREF)
             .AsNoTracking()
             .FirstOrDefault(x => x.GroupUserXREF.Count > 0);
        var userId = group.GroupUserXREF.First().UserId;

        // Act
        await _groupService.RemoveUserFromGroup(group.Id, userId);
        var users = _groupService.GetUsersByGroupId(group.Id);
        // Assert
        Assert.IsFalse(users.Any(x=>x.Id == userId));
    }

    [TestMethod]
    public async Task Test_RemoveUserFromGroup_WhenGroupDoesNotExist()
    {
        // Arrange
        int groupId = 999333333;
        int userId = 1;

        // Act and Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await _groupService.RemoveUserFromGroup(groupId, userId));
    }

    [TestMethod]
    public async Task Test_RemoveUserFromGroup_WhenUserDoesNotBelongToGroup()
    {
        // Arrange
        int groupId = 1;
        int userId = 999;

        // Act and Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await _groupService.RemoveUserFromGroup(groupId, userId));
    }

    [TestMethod]
    public async Task Test_RemoveUserFromGroup_WhenGroupIdOrUserIdIsInvalid()
    {
        // Arrange
        int groupId = -1;
        int userId = -1;

        // Act and Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await _groupService.RemoveUserFromGroup(groupId, userId));
    }

    [TestMethod]
    public void GetGroupMessagesGroupedByDate_ThrowsException_IfUserNotMemberOfGroup()
    {
        // Arrange
        int groupId = 1;
        int currentUserId = 9999; // user not a member of group
        int pageIndex = 0;
        int pageSize = 10;

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => _groupService.GetGroupMessagesGroupedByDate(groupId, currentUserId, pageIndex, pageSize));
    }

    [TestMethod]
    public void GetGroupMessagesGroupedByDate_ReturnsGroupedMessages_IfUserIsMemberOfGroup()
    {
        // Arrange
        int pageIndex = 0;
        int pageSize = 10;

        var createGroup = MockGroupData.GetAGroup();
        _dbContext.Groups.Add(createGroup);
        _dbContext.SaveChanges();

        var group = _dbContext.Groups
            .Include(x=>x.GroupUserXREF)
            .ThenInclude(x=>x.User)
            .Include(x=>x.GroupMessages)
            .ThenInclude(x=>x.MessageLikes)
            .AsNoTracking()
            .First(x => x.GroupMessages.Count > 0 && x.GroupUserXREF.Count() > 0);
        var userId = group.GroupUserXREF.First().UserId;

        // Act
        var result = _groupService.GetGroupMessagesGroupedByDate(group.Id, userId, pageIndex, pageSize);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IEnumerable<GroupedGroupMessageResponse>));
        Assert.IsTrue(result.Any());
    }

    [TestMethod]
    public async Task SendMessageToGroup_ShouldReturnTrue()
    {
        // Arrange
        var createGroup = MockGroupData.GetAGroup();
        _dbContext.Groups.Add(createGroup);
        _dbContext.SaveChanges();
        var messageText = Guid.NewGuid().ToString();

        var group = _dbContext.Groups
           .Include(x => x.GroupUserXREF)
           .ThenInclude(x => x.User)
           .Include(x => x.GroupMessages)
           .ThenInclude(x => x.MessageLikes)
           .AsNoTracking()
           .First(x => x.GroupMessages.Count > 0 && x.GroupUserXREF.Count() > 0);
        var userId = group.GroupUserXREF.First().UserId;

        // Act
        await _groupService.SendMessageToGroup(group.Id, userId, messageText);

        // Assert
        var message = _dbContext.GroupMessages.FirstOrDefault(x=>x.Text == messageText);
        Assert.IsNotNull(message);
        Assert.AreEqual(group.Id, message.GroupId);
        Assert.AreEqual(userId, message.SenderId);
        Assert.AreEqual(messageText, message.Text);
    }
}
