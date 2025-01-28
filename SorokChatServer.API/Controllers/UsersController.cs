using Microsoft.AspNetCore.Mvc;
using SorokChatServer.Core.Contracts;
using SorokChatServer.Core.Interfaces;

namespace SorokChatServer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest newUser, CancellationToken cancellationToken)
    {
        var createdResult = await _usersService.Create(newUser, cancellationToken);
        if (createdResult.IsFailure)
        {
            var error = createdResult.Error;
            return StatusCode((int)error.StatusCode, error.Message);
        }

        const string uri = "/users/created";
        return Created(uri, createdResult.Value);
    }
}