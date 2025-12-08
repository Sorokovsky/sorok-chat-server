using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using SorokChatServer.Domain.Models;

namespace SorokChatServer.Domain.Services;

public class CookieTokenStorage : IRefreshTokenStorage
{
    private const string CookieName = "__Host-refresh-token";
    private readonly ITokenDeserializer _tokenDeserializer;

    private readonly ITokenSerializer _tokenSerializer;

    public CookieTokenStorage(ITokenSerializer tokenSerializer, ITokenDeserializer tokenDeserializer)
    {
        _tokenSerializer = tokenSerializer;
        _tokenDeserializer = tokenDeserializer;
    }

    public Result<Token, Error> GetToken(HttpRequest request)
    {
        var cookie =
            request.Cookies.FirstOrDefault(x => x.Key.Equals(CookieName));
        return _tokenDeserializer.Deserialize(cookie.Value);
    }

    public void SetToken(Token token, HttpResponse response)
    {
        var stringTokenResult = _tokenSerializer.Serialize(token);
        if (stringTokenResult.IsFailure) return;
        response.Cookies.Append(CookieName, stringTokenResult.Value,
            GenerateCookieOptions(token.ExpiresAt - token.CreatedAt));
    }

    public void ClearToken(HttpResponse response)
    {
        response.Cookies.Delete(CookieName);
    }

    private static CookieOptions GenerateCookieOptions(TimeSpan maxAge)
    {
        return new CookieOptions
        {
            Domain = null,
            MaxAge = maxAge,
            Secure = true,
            HttpOnly = true,
            Path = "/"
        };
    }
}