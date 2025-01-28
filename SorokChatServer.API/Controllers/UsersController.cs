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
    public async Task<IActionResult> Create([FromForm] CreateUserRequest newUser, CancellationToken cancellationToken)
    {
        Console.WriteLine(newUser.avatar);
        var createdResult = await _usersService.Create(newUser, cancellationToken);
        if (createdResult.IsFailure)
        {
            var error = createdResult.Error;
            return StatusCode((int)error.StatusCode, error.Message);
        }

        const string uri = "/users/created";
        var user = createdResult.Value;
        var response = new UserResponse(
            user.Id,
            user.CreatedAt,
            user.UpdatedAt,
            user.Email.Value,
            user.Surname,
            user.Name,
            user.MiddleName,
            user.AvatarPath
        );
        return Created(uri, response);
    }
}