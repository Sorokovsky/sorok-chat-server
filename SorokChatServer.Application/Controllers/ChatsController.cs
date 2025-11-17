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

    public ChatsController(IChatsService chatsService)
    {
        _chatsService = chatsService;
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateChat chat, [FromServices] ICurrentUserService currentUser,
        CancellationToken cancellationToken = default)
    {
        var createdChat = await _chatsService.CreateAsync(chat, currentUser.Current!, cancellationToken);
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

    [HttpPut("add-member/{chatId:long}/{memberId:long}")]
    public async Task<IActionResult> AddMember([FromRoute] long chatId, [FromRoute] long memberId,
        CancellationToken cancellationToken = default)
    {
        var chat = await _chatsService.AddUserAsync(chatId, memberId, cancellationToken);
        if (chat.IsFailure) return BadRequest(chat.Error);
        return Ok(chat.Value.ToGet());
    }

    [HttpPut("remove-member/{chatId:long}/{memberId:long}")]
    public async Task<IActionResult> RemoveMember([FromRoute] long chatId, [FromRoute] long memberId,
        CancellationToken cancellationToken = default)
    {
        var chat = await _chatsService.RemoveUserAsync(chatId, memberId, cancellationToken);
        if (chat.IsFailure) return BadRequest(chat.Error);
        return Ok(chat.Value.ToGet());
    }

    [HttpPut("write-message/{chatId:long}")]
    public async Task<IActionResult> WriteMessage(
        [FromRoute] long chatId,
        [FromServices] ICurrentUserService currentUser,
        [FromBody] CreateMessage message,
        CancellationToken cancellationToken = default
    )
    {
        var chat = await _chatsService.AddMessageAsync(chatId, currentUser.Current!.Id, message, cancellationToken);
        if (chat.IsFailure) return BadRequest(chat.Error);
        return Ok(chat.Value.ToGet());
    }

    [HttpPut("remove-message/{chatId:long}/{messageId:long}")]
    public async Task<IActionResult> RemoveMessage(
        [FromRoute] long chatId,
        [FromRoute] long messageId,
        CancellationToken cancellationToken = default
    )
    {
        var chat = await _chatsService.RemoveMessageAsync(chatId, messageId, cancellationToken);
        if (chat.IsFailure) return BadRequest(chat.Error);
        return Ok(chat.Value.ToGet());
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken = default)
    {
        var chat = await _chatsService.DeleteAsync(id, cancellationToken);
        if (chat.IsFailure) return BadRequest(chat.Error);
        return Ok(chat.Value.ToGet());
    }
}