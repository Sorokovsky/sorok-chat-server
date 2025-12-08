using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using SorokChatServer.Domain.Contracts;
using SorokChatServer.Domain.Contracts.User;
using SorokChatServer.Domain.Services;

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

    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] NewUser newUser, CancellationToken cancellationToken)
    {
        var (isSuccess, _, value, error) = await _authenticationService.RegisterAsync(newUser, cancellationToken);
        if (isSuccess) return Ok(value);
        return StatusCode((int)error.StatusCode, error);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUser loginUser,
        CancellationToken cancellationToken
        )
    {
        var (isSuccess, _, user, error) = await _authenticationService.LoginAsync(loginUser, cancellationToken);
        return isSuccess ? Ok(user) : StatusCode((int)error.StatusCode, error);
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await _authenticationService.LogoutAsync(cancellationToken);
        return NoContent();
    }
    
}