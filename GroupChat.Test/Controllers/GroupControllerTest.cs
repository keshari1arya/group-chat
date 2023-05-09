using System.Security.Claims;
using System.Security.Principal;
using GroupChat.Controllers;
using GroupChat.Dto;
using GroupChat.Models;
using GroupChat.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;

namespace GroupChat.Tests.Controllers;

[TestClass]
public class GroupsControllerTests
{
    private Mock<ILogger<GroupController>> _loggerMock;
    private Mock<IGroupService> _groupServiceMock;
    private GroupController _groupsController;

    [TestInitialize]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<GroupController>>();
        _groupServiceMock = new Mock<IGroupService>();
        _groupsController = new GroupController(_loggerMock.Object, _groupServiceMock.Object);

        var mockHttpContext = new Mock<HttpContext>();
        var mockIdentity = new GenericIdentity("test");
        var mockClaimsPrincipal = new ClaimsPrincipal(mockIdentity);
        mockClaimsPrincipal.AddIdentity(new ClaimsIdentity(new List<Claim> { new Claim("Id", "1") }));
        mockHttpContext.SetupGet(x => x.User).Returns(mockClaimsPrincipal);
        _groupsController.ControllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object
        };
    }

    [TestMethod]
    public async Task ShoudGetAllGroups_ReturnsOk()
    {
        // Arrange
        var groups = new List<GroupResponse> {
            new GroupResponse { Id = 1, Name = "Group 1" },
            new GroupResponse { Id = 2, Name = "Group 2" } };
        _groupServiceMock.Setup(svc => svc.GetAllGroups()).ReturnsAsync(groups);

        // Act
        var result = await _groupsController.GetAllGroups();

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        Assert.AreEqual(groups, okResult.Value);
    }

    [TestMethod]
    public async Task ShoudGetGroupById_WithValidId_ReturnsOk()
    {
        // Arrange
        var groupId = 1;
        var group = new GroupResponse { Id = groupId, Name = "Group 1" };
        _groupServiceMock.Setup(svc => svc.GetGroupById(groupId)).ReturnsAsync(group);

        // Act
        var result = await _groupsController.GetGroupById(groupId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        Assert.AreEqual(group, okResult.Value);
    }

    [TestMethod]
    public async Task ShoudNotGetGroupById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var groupId = 1;
        _groupServiceMock.Setup(svc => svc.GetGroupById(groupId)).ReturnsAsync((GroupResponse)null);

        // Act
        var result = await _groupsController.GetGroupById(groupId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task ShoudCreateGroup_WithValidGroup_ReturnsCreatedAtAction()
    {
        // Arrange
        var group = new GroupRequest { Id = 1, Name = "Group 1" };

        // Act
        var result = await _groupsController.CreateGroup(group);

        // Assert
        Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        var createdAtActionResult = (CreatedAtActionResult)result;
        Assert.AreEqual("GetGroupById", createdAtActionResult.ActionName);
        Assert.AreEqual(group.Id, createdAtActionResult.RouteValues["id"]);
        Assert.AreEqual(group, createdAtActionResult.Value);
    }

    [TestMethod]
    public async Task ShoudNotUpdateGroup_WithValidGroup_ReturnsNoContent()
    {
        // Arrange
        var groupId = 1;
        var group = new GroupRequest { Id = groupId, Name = "Group 1" };
        _groupServiceMock.Setup(svc => svc.UpdateGroup(group)).Returns(Task.CompletedTask);

        // Act
        var result = await _groupsController.UpdateGroup(groupId, group);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }

    [TestMethod]
    public async Task ShoudNotUpdateGroup_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var groupId = 123;
        var group = new GroupRequest { Id = groupId, Name = "Updated Group" };
        _groupServiceMock.Setup(s => s.UpdateGroup(group)).Throws(new KeyNotFoundException());

        // Act
        var result = await _groupsController.UpdateGroup(groupId, group);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult)); 
    }

    [TestMethod]
    public async Task ShoudNotUpdateGroup_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var groupId = 1;
        var group = new GroupRequest { Id = groupId, Name = null };
        _groupsController.ModelState.AddModelError("Name", "The Name field is required.");

        // Act
        var result = await _groupsController.UpdateGroup(groupId, group);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }



    [TestMethod]
    public void ShouldGetGroupMessages_ReturnsOkResult_WhenServiceReturnsMessages()
    {
        // Arrange
        int groupId = 1;
        int userId = 1;
        int page = 0;
        int pageSize = 20;

        var messages =new List<GroupedGroupMessageResponse> { 
            new GroupedGroupMessageResponse
            {
                Date = DateTime.Now,
                        Messages = new List<GroupMessageResponse>
                {
                    new GroupMessageResponse { Id = 1, GroupId = 1,  Text = "Message 1" },
                    new GroupMessageResponse { Id = 2, GroupId = 1, Text = "Message 2" },
                    new GroupMessageResponse { Id = 3, GroupId = 1,  Text = "Message 3" },
                }
            }
        };
        _groupServiceMock.Setup(x => x.GetGroupMessagesGroupedByDate(groupId, userId, page, pageSize)).Returns(messages);
        

        // Act
        var result = _groupsController.GetGroupMessages(groupId, page, pageSize) ;

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        Assert.AreEqual(messages, okResult.Value);
    }

    //[TestMethod]
    //public void ShouldNotGetGroupMessages_ReturnsBadRequestResult_WhenServiceThrowsInvalidOperationException()
    //{
    //    // Arrange
    //    int groupId = 1;
    //    int userId = 2;
    //    int page = 0;
    //    int pageSize = 20;
    //    var errorMessage = "An error occurred";

    //    _groupServiceMock.Setup(x => x.GetGroupMessagesGroupedByDate(groupId, userId, page, pageSize))
    //        .Throws(new InvalidOperationException());

    //    // Act
    //    var result = _groupsController.GetGroupMessages(groupId, page, pageSize);

    //    // Assert
    //    Assert.IsNotNull(result);
    //    Assert.IsInstanceOfType(result,typeof( BadRequestResult));
    //}
}
