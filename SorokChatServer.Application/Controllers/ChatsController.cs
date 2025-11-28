using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Application.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ChatsController : ControllerBase
{
    private readonly IChatsService _chatsService;
    private readonly IUsersService _usersService;

    public ChatsController(IChatsService chatsService, IUsersService usersService)
    {
        _chatsService = chatsService;
        _usersService = usersService;
    }

    [HttpGet("by-me")]
    public async Task<IActionResult> GetByMe([FromServices] ICurrentUserService currentUser,
        CancellationToken cancellationToken = default)
    {
        var chats = await _chatsService.GetByUserAsync(currentUser.Current!.Id, cancellationToken);
        return Ok(chats.Select(chat => chat.ToGet()).ToList());
    }

    [HttpGet("by-id/{id:long}")]
    public async Task<IActionResult> GetById([FromRoute] long id, CancellationToken cancellationToken = default)
    {
        var chat = await _chatsService.GetByIdAsync(id, cancellationToken);
        if (chat.IsFailure) return BadRequest(chat.Error);
        return Ok(chat.Value.ToGet());
    }

    [HttpGet("by-title/{title}")]
    public async Task<IActionResult> GetByName([FromRoute] string title, CancellationToken cancellationToken = default)
    {
        var chat = await _chatsService.GetByTitleAsync(title, cancellationToken);
        if (chat.IsFailure) return BadRequest(chat.Error);
        return Ok(chat.Value.ToGet());
    }

    [HttpPost("{opponentEmail}")]
    public async Task<IActionResult> Create([FromBody] CreateChat chat, [FromRoute] string opponentEmail,
        [FromServices] ICurrentUserService currentUser,
        CancellationToken cancellationToken = default)
    {
        var opponentResult = await _usersService.GetByEmailAsync(opponentEmail, cancellationToken);
        if (opponentResult.IsFailure) return BadRequest(opponentResult.Error);
        if (opponentResult.Value.Id == currentUser.Current!.Id)
            return BadRequest("Не можна створити чат з самим собою лише.");
        var createdChat =
            await _chatsService.CreateAsync(chat, currentUser.Current!, opponentResult.Value, cancellationToken);
        if (createdChat.IsFailure) return BadRequest(createdChat.Error);
        return Created($"/chats/{createdChat.Value.Id}", createdChat.Value.ToGet());
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update([FromRoute] long id, [FromBody] UpdateChat chat,
        CancellationToken cancellationToken = default)
    {
        var updatedChat = await _chatsService.UpdateAsync(id, chat, cancellationToken);
        if (updatedChat.IsFailure) return BadRequest(updatedChat.Error);
        return Ok(updatedChat.Value.ToGet());
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken = default)
    {
        var chat = await _chatsService.DeleteAsync(id, cancellationToken);
        if (chat.IsFailure) return BadRequest(chat.Error);
        return Ok(chat.Value.ToGet());
    }
}