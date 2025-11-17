using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUser createdUser,
        CancellationToken cancellationToken = default)
    {
        var result = await _authenticationService.RegisterAsync(createdUser, cancellationToken);
        if (result.IsFailure) return BadRequest(result.Error);
        var location = "/authentication/get-me";
        return Created(location, result.Value.ToGet());
    }

    [HttpPut("login")]
    public async Task<IActionResult> Login([FromBody] LoginUser loginUser,
        CancellationToken cancellationToken = default)
    {
        var result = await _authenticationService.LoginAsync(loginUser, cancellationToken);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value.ToGet());
    }

    [HttpDelete("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken = default)
    {
        await _authenticationService.LogoutAsync(cancellationToken);
        return NoContent();
    }

    [HttpGet("get-me")]
    [Authorize]
    public async Task<IActionResult> GetMe([FromServices] ICurrentUserService currentUser,
        CancellationToken cancellationToken = default)
    {
        return Ok(currentUser.Current!.ToGet());
    }
}