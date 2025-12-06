using Microsoft.AspNetCore.Http;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Core.Services;

public class RefreshTokenStorage : IRefreshTokenStorage
{
    private const string CookieName = "__Host-refresh-token";

    private readonly ITokenSerializerService _tokenSerializerService;

    public RefreshTokenStorage(ITokenSerializerService tokenSerializerService)
    {
        _tokenSerializerService = tokenSerializerService;
    }

    public async Task SetTokenAsync(Token token, HttpResponse response, CancellationToken cancellationToken = default)
    {
        var stringToken = await _tokenSerializerService.SerializeTokenAsync(token, cancellationToken);
        response.Cookies.Append(CookieName, stringToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Domain = null,
            Path = "/",
            MaxAge = token.ExpiresAt - token.CreatedAt
        });
    }

    public async Task<Token?> GetTokenAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        var cookie = request.Cookies[CookieName];
        if (cookie is null) return null;
        var token = await _tokenSerializerService.DeserializeTokenAsync(cookie, cancellationToken);
        return token.IsFailure ? null : token.Value;
    }

    public Task ClearTokenAsync(HttpResponse response, CancellationToken cancellationToken = default)
    {
        response.Cookies.Delete(CookieName, new CookieOptions
        {
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Domain = null,
            Path = "/"
        });
        return Task.CompletedTask;
    }
}