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
        var header = request.Headers[Authorization].FirstOrDefault() ?? string.Empty;
        var stringToken = header.Replace(Bearer, string.Empty);

        var result = await _tokenSerializerService.DeserializeTokenAsync(stringToken, cancellationToken);
        if (result.IsFailure) return null;
        return result.Value;
    }

    public Task ClearTokenAsync(HttpResponse response, CancellationToken cancellationToken = default)
    {
        response.Headers.Remove(Authorization);
        return Task.CompletedTask;
    }
}