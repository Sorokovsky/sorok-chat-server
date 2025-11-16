using Microsoft.AspNetCore.Http;
using SorokChatServer.Logic.Models;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Core.Middlewares;

public class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public JwtAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    public async Task InvokeAsync(
        HttpContext context,
        RequestDelegate next,
        IAccessTokenStorage accessTokenStorage,
        IRefreshTokenStorage refreshTokenStorage,
        IUsersService usersService,
        CancellationToken cancellationToken = default
    )
    {
        var accessToken = await accessTokenStorage.GetTokenAsync(context.Request, cancellationToken);
        var refreshToken = await refreshTokenStorage.GetTokenAsync(context.Request, cancellationToken);
        if (accessToken is not null)
        {
            var user = await usersService.GetByEmailAsync(accessToken.Email, cancellationToken);
            if (user.IsSuccess) context.Items[nameof(User)] = user.Value;
        }
    }
}