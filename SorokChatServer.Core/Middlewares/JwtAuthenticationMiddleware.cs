using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SorokChatServer.Core.Options;
using SorokChatServer.Logic.Contracts;
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
        IOptions<JwtOptions> jwtOptions,
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
        else
        {
            if (refreshToken is not null)
            {
                var user = await usersService.GetByEmailAsync(refreshToken.Email, cancellationToken);
                if (user.IsSuccess)
                {
                    context.Items[nameof(User)] = user.Value;
                    var now = DateTime.UtcNow;
                    var email = user.Value.Email.Value;
                    var accessExpiresAt = now.AddMinutes(jwtOptions.Value.AccessTokenLifetimeMinutes);
                    var refreshExpiresAt = now.AddDays(jwtOptions.Value.RefreshTokenLifetimeDays);
                    var newAccess = new Token(Guid.NewGuid(), email, now, accessExpiresAt);
                    var newRefresh = new Token(Guid.NewGuid(), email, now, refreshExpiresAt);
                    await accessTokenStorage.SetTokenAsync(newAccess, context.Response, cancellationToken);
                    await refreshTokenStorage.SetTokenAsync(newRefresh, context.Response, cancellationToken);
                }
            }
        }
        await next(context);
    }
}