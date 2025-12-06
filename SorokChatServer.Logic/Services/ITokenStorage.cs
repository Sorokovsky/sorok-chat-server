using Microsoft.AspNetCore.Http;
using SorokChatServer.Logic.Contracts;

namespace SorokChatServer.Logic.Services;

public interface ITokenStorage
{
    public Task SetTokenAsync(Token token, HttpResponse response, CancellationToken cancellationToken = default);
    public Task<Token?> GetTokenAsync(HttpRequest request, CancellationToken cancellationToken = default);
    public Task ClearTokenAsync(HttpResponse response, CancellationToken cancellationToken = default);
}