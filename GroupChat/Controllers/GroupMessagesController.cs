using GroupChat.Models;
using GroupChat.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GroupChat.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GroupMessagesController : ControllerBase
{
    private readonly ChatDbContext _context;

    public GroupMessagesController(ChatDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupMessage>>> GetGroupMessages()
    {
        return await _context.GroupMessages.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupMessage>> GetGroupMessage(int id)
    {
        var groupMessage = await _context.GroupMessages.FindAsync(id);

        if (groupMessage == null)
        {
            return NotFound();
        }

        return groupMessage;
    }

    [HttpPost]
    public async Task<ActionResult<GroupMessage>> CreateGroupMessage(GroupMessage groupMessage)
    {
        _context.GroupMessages.Add(groupMessage);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGroupMessage), new { id = groupMessage.Id }, groupMessage);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroupMessage(int id, GroupMessage groupMessage)
    {
        if (id != groupMessage.Id)
        {
            return BadRequest();
        }

        _context.Entry(groupMessage).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GroupMessageExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroupMessage(int id)
    {
        var groupMessage = await _context.GroupMessages.FindAsync(id);
        if (groupMessage == null)
        {
            return NotFound();
        }

        _context.GroupMessages.Remove(groupMessage);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool GroupMessageExists(int id)
    {
        return _context.GroupMessages.Any(e => e.Id == id);
    }
}