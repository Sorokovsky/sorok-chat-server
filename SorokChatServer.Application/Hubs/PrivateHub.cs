using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.SignalR;
using SorokChatServer.Logic.Models;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Application.Hubs;

public abstract class PrivateHub<T> : Hub<T> where T : class
{
    private readonly ITokenSerializerService _tokenSerializerService;
    private readonly IUsersService _usersService;

    protected PrivateHub(IUsersService usersService, ITokenSerializerService tokenSerializerService)
    {
        _usersService = usersService;
        _tokenSerializerService = tokenSerializerService;
    }

    protected async Task<Result<User>> ExtractUserFromAccessToken(
        string accessToken,
        CancellationToken cancellationToken = default
    )
    {
        var deserializationResult = await _tokenSerializerService.DeserializeTokenAsync(accessToken, cancellationToken);
        if (deserializationResult.IsFailure) return Result.Failure<User>(deserializationResult.Error);
        return await _usersService.GetByEmailAsync(deserializationResult.Value.Email, cancellationToken);
    }
}