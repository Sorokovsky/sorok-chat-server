using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SorokChatServer.Core.Options;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Models;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Core.Handlers;

public class JwtHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IAccessTokenStorage _accessTokenStorage;
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly IRefreshTokenStorage _refreshTokenStorage;
    private readonly IUsersService _usersService;

    public JwtHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock, IUsersService usersService, IAccessTokenStorage accessTokenStorage,
        IRefreshTokenStorage refreshTokenStorage, IOptions<JwtOptions> jwtOptions,
        ITokenSerializerService tokenSerializerService)
        : base(options, logger, encoder, clock)
    {
        _usersService = usersService;
        _accessTokenStorage = accessTokenStorage;
        _refreshTokenStorage = refreshTokenStorage;
        _jwtOptions = jwtOptions;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var cancellationToken = Context.RequestAborted;
        var accessToken = await _accessTokenStorage.GetTokenAsync(Request, cancellationToken);
        var refreshToken = await _refreshTokenStorage.GetTokenAsync(Request, cancellationToken);
        if (accessToken is not null || refreshToken is not null)
        {
            var validToken = accessToken ?? refreshToken;
            var userResult = await _usersService.GetByEmailAsync(validToken!.Email, cancellationToken);
            if (userResult.IsFailure) return AuthenticateResult.NoResult();
            var now = DateTime.UtcNow;
            var accessExpires = now.AddMinutes(_jwtOptions.Value.AccessTokenLifetimeMinutes);
            var email = userResult.Value.Email;
            var refreshExpires = now.AddDays(_jwtOptions.Value.RefreshTokenLifetimeDays);
            if (refreshToken is not null)
            {
                var newAccess = new Token(Guid.NewGuid(), email.Value, now, accessExpires);
                var newRefresh = new Token(Guid.NewGuid(), email.Value, now, refreshExpires);
                await _accessTokenStorage.SetTokenAsync(newAccess, Response, cancellationToken);
                await _refreshTokenStorage.SetTokenAsync(newRefresh, Response, cancellationToken);
                Context.Items[nameof(User)] = userResult.Value;
                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, email.Value),
                    new Claim(ClaimTypes.NameIdentifier, userResult.Value.Id.ToString())
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
        }

        return AuthenticateResult.NoResult();
    }
}