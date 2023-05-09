using GroupChat.Controllers;
using GroupChat.Dto;
using GroupChat.Models;
using GroupChat.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupChat.Tests.Controllers;

[TestClass]
public class UsersControllerTest
{
    private UsersController _controller;
    private Mock<IUserService> _userServiceMock;

    [TestInitialize]
    public void Initialize()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new UsersController(_userServiceMock.Object);
    }

    [TestMethod]
    public async Task GetUsers_ReturnsOkResult_WithListOfUsers()
    {
        // Arrange
        var expectedUsers = new List<User> { new User { Id = 1, Name = "John" }, new User { Id = 2, Name = "Jane" } };
        _userServiceMock.Setup(x => x.GetAllUsers()).ReturnsAsync(expectedUsers);

        // Act
        var result = await _controller.GetUsers();

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result.Result;
        Assert.AreEqual(expectedUsers.Count, ((IEnumerable<User>)okResult.Value).Count());
        Assert.IsTrue(expectedUsers.SequenceEqual((IEnumerable<User>)okResult.Value));
    }

    [TestMethod]
    public async Task GetUser_WithValidId_ReturnsOkResult_WithUser()
    {
        // Arrange
        var expectedUser = new User { Id = 1, Name = "John" };
        _userServiceMock.Setup(x => x.GetUserById(expectedUser.Id)).ReturnsAsync(expectedUser);

        // Act
        var result = await _controller.GetUser(expectedUser.Id);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result.Result;
        Assert.AreEqual(expectedUser, okResult.Value);
    }

    [TestMethod]
    public async Task GetUser_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        var invalidId = 99;
        _userServiceMock.Setup(x => x.GetUserById(invalidId)).ReturnsAsync(null as User);

        // Act
        var result = await _controller.GetUser(invalidId);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task CreateUser_WithValidUser_ReturnsCreatedAtActionResult_WithNewUser()
    {
        // Arrange
        var newUser = new UserRequest { Name = "John" };
        var createdUser = new User { Id = 1, Name = newUser.Name };
        _userServiceMock.Setup(x => x.CreateUser(newUser)).ReturnsAsync(createdUser);

        // Act
        var result = await _controller.CreateUser(newUser);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        var createdAtActionResult = (CreatedAtActionResult)result.Result;
        Assert.AreEqual(nameof(UsersController.GetUser), createdAtActionResult.ActionName);
        Assert.AreEqual(createdUser.Id, createdAtActionResult.RouteValues["id"]);
        Assert.AreEqual(createdUser, createdAtActionResult.Value);
    }

    [TestMethod]
    public async Task UpdateUser_WithValidUser_ReturnsNoContentResult()
    {
        // Arrange
        var userId = 1;
        var user = new UserRequest { Name = "John Smith", Email = "john@example.com" };

        // Act
        var result = await _controller.UpdateUser(userId, user);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }

    [TestMethod]
    public async Task UpdateUser_WithInvalidUser_ReturnsBadRequestResult()
    {
        // Arrange
        var userId = 1;
        var user = new UserRequest { Name = "", Email = "invalid-email" };
        _controller.ModelState.AddModelError("Name", "The Name field is required.");

        // Act
        var result = await _controller.UpdateUser(userId, user);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task UpdateUser_WithNonExistingUser_ReturnsNotFoundResult()
    {
        // Arrange
        var userId = 1;
        var user = new UserRequest { Name = "John Smith", Email = "john@example.com" };
        _userServiceMock.Setup(s => s.UpdateUser(user))
            .ThrowsAsync(new KeyNotFoundException($"User with id {userId} not found"));

        // Act
        var result = await _controller.UpdateUser(userId, user);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task DeleteUser_WithValidId_ReturnsNoContentResult()
    {
        // Arrange
        var userId = 1;

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }

    [TestMethod]
    public async Task DeleteUser_WithNonExistingId_ReturnsNotFoundResult()
    {
        // Arrange
        var userId = 1;
        _userServiceMock.Setup(s => s.DeleteUser(userId))
            .ThrowsAsync(new KeyNotFoundException($"User with id {userId} not found"));

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }
}