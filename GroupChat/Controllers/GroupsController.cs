using GroupChat.Dto;
using GroupChat.Models;
using GroupChat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupChat.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly ILogger<GroupsController> _logger;
    private readonly IGroupService _groupService;

    public GroupsController(ILogger<GroupsController> logger, IGroupService groupService)
    {
        _logger = logger;
        _groupService = groupService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGroups()
    {
        var groups = await _groupService.GetAllGroups();
        return Ok(groups);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroupById(int id)
    {
        var group = await _groupService.GetGroupById(id);
        if (group == null)
        {
            return NotFound();
        }
        return Ok(group);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] Group group)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _groupService.CreateGroup(group);
        return CreatedAtAction(nameof(GetGroupById), new { id = group.Id }, group);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(int id, [FromBody] Group group)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (id != group.Id)
        {
            return BadRequest();
        }
        try
        {
            await _groupService.UpdateGroup(group);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        try
        {
            await _groupService.DeleteGroup(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("{id}/users")]
    public IActionResult GetUsersByGroupId(int id)
    {
        var users = _groupService.GetUsersByGroupId(id);
        return Ok(users);
    }

    [HttpPost("{groupId}/users/{userId}")]
    public async Task<IActionResult> AddUserToGroup(int groupId, int userId)
    {
        try
        {
            await _groupService.AddUserToGroup(groupId, userId);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok("User Added successfully");
    }

    [HttpDelete("{groupId}/users/{userId}")]
    public async Task<IActionResult> RemoveUserFromGroup(int groupId, int userId)
    {
        try
        {
            bool removed = await _groupService.RemoveUserFromGroup(groupId, userId);
            if (!removed)
            {
                return NotFound();
            }
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok("User Removed Successfully");
    }

    [HttpGet("{id}/messages")]
    public async Task<ActionResult<IEnumerable<GroupMessage>>> GetMessagesForGroup(int id)
    {
        var messages = _groupService.GetGroupMessages(id);
        return Ok(messages);
    }

    [HttpPost("{groupId}/messages")]
    public async Task<IActionResult> SendMessageToGroup(int groupId, [FromBody] MessageRequest request)
    {
        try
        {
            await _groupService.SendMessageToGroup(groupId, request.SenderId, request.Message);
            return Ok("Message sent successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("users/{userId}/messages/{messageId}/like")]
    public async Task<ActionResult> LikeGroupMessage(int userId, int messageId)
    {
        try
        {
            await _groupService.ToggleLikeMessage(userId, messageId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


}
