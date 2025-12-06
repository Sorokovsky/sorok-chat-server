using Microsoft.AspNetCore.Http;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Core.Services;

public class AccessTokenStorage : IAccessTokenStorage
{
    private const string Authorization = nameof(Authorization);
    private const string Bearer = nameof(Bearer);

    private readonly ITokenSerializerService _tokenSerializerService;

    public AccessTokenStorage(ITokenSerializerService tokenSerializerService)
    {
        _tokenSerializerService = tokenSerializerService;
    }

    public async Task SetTokenAsync(Token token, HttpResponse response, CancellationToken cancellationToken = default)
    {
        var stringToken = await _tokenSerializerService.SerializeTokenAsync(token, cancellationToken);
        response.Headers[Authorization] = $"{Bearer} {stringToken}";
    }

    public async Task<Token?> GetTokenAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        string? rawToken = null;
        var authorizationHeader = request.Headers[Authorization].FirstOrDefault();
        if (!string.IsNullOrEmpty(authorizationHeader) &&
            authorizationHeader.StartsWith(Bearer, StringComparison.OrdinalIgnoreCase))
            rawToken = authorizationHeader[Bearer.Length..].Trim();

        if (string.IsNullOrEmpty(rawToken) && request.Query.ContainsKey("access_token"))
            rawToken = request.Query["access_token"].FirstOrDefault();

        if (string.IsNullOrEmpty(rawToken)) return null;
        var result = await _tokenSerializerService.DeserializeTokenAsync(rawToken, cancellationToken);
        return result.IsSuccess ? result.Value : null;
    }

    public Task ClearTokenAsync(HttpResponse response, CancellationToken cancellationToken = default)
    {
        response.Headers.Remove(Authorization);
        return Task.CompletedTask;
    }
}