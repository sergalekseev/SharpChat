using Microsoft.AspNetCore.Mvc;
using SharpChat.Core.DataContracts;
using SharpChat.Core.Services;
using SharpChat.Server.Services;

namespace SharpChat.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthManager _authManager;

    public AuthController(IAuthManager authService)
    {
        _authManager = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var result = await _authManager.LoginAsync(loginDto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()//[FromBody] RefreshTokenDto dto)
    {
        // TODO: Implement refresh logic
        return Ok();
    }
}

