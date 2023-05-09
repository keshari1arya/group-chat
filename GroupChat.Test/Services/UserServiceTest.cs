using GroupChat.Dto;
using GroupChat.Services;
using GroupChat.Test;
using Microsoft.EntityFrameworkCore;

namespace GroupChat.Test.Services;

[TestClass]
public class UserServiceTest : BaseTest
{
    private readonly IUserService _userService;

    public UserServiceTest()
    {
        _userService = new UserService(_dbContext, _mapper);
    }

    [TestInitialize]
    public void Initialize()
    {
        Init();
    }

    [TestMethod]
    public async Task GetUserById_Returns_User_With_Correct_Id()
    {
        // Arrange


        // Act
        var result = await _userService.GetUserById(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
    }

    [TestMethod]
    public async Task GetAllUsers_Returns_All_Users()
    {
        // Arrange
        var users = _dbContext.Users.ToList();
        // Act
        var result = await _userService.GetAllUsers();

        // Assert
        Assert.AreEqual(users.Count, result.Count());
        Assert.IsTrue(result.All(x => users.Any(y => y.Id == x.Id)));
    }

    [TestMethod]
    public async Task CreateUser_Creates_New_User()
    {
        // Arrange
        var userRequest = new UserRequest { Username = "john", Password = "pass", Email = "", Name = "john" };


        // Act
        var result = await _userService.CreateUser(userRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(userRequest.Username, result.Username);
        Assert.AreEqual(userRequest.Password, result.Password);
        Assert.IsTrue(result.Id > 0);
        Assert.IsTrue(_dbContext.Users.Any(x => x.Id == result.Id));
    }

    [TestMethod]
    public async Task UpdateUser_Updates_User_With_Correct_Id()
    {
        // Arrange
        var userRequest = new UserRequest { Username = "john", Password = "pass", Email = "", Name = "john" };
        var user = await _userService.CreateUser(userRequest);
        var updatedUser = _mapper.Map<UserRequest>(user);

        // Act
        _dbContext.Entry(user).State = EntityState.Detached;
        await _userService.UpdateUser(updatedUser);

        // Assert
        var result = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == user.Id);
        Assert.IsNotNull(result);
        Assert.AreEqual(updatedUser.Username, result.Username);
        Assert.AreEqual(updatedUser.Password, result.Password);
    }
}