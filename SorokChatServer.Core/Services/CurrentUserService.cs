using Microsoft.AspNetCore.Http;
using SorokChatServer.Logic.Models;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Core.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public User? Current => IsAuthenticated ? _httpContextAccessor.HttpContext!.Items[nameof(User)] as User : null;
}