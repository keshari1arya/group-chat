using GroupChat.Dto;
using GroupChat.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroupChat.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IJwtAuthenticationService _jwtAuthenticationService;
    private readonly IUserService _userService;
    public AuthController(IJwtAuthenticationService jwtAuthenticationService, IUserService userService)
    {
        _jwtAuthenticationService = jwtAuthenticationService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = await _userService.Authenticate(model.Username, model.Password);

        if (user == null)
        {
            return Unauthorized();
        }

        var token = _jwtAuthenticationService.Authenticate(user);

        return Ok(new { Token = token });
    }

}