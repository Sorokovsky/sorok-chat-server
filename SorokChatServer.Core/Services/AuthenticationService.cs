using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SorokChatServer.Core.Options;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Models;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Core.Services;

public class AuthenticationService : IAuthenticationService
{
    private const string Authorization = nameof(Authorization);
    private const string Bearer = nameof(Bearer);
    private const string CookieName = "__Host-token";
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtOptions _options;
    private readonly ITokenSerializerService _tokenSerializerService;

    private readonly IUsersService _usersService;

    public AuthenticationService(IUsersService usersService, IHttpContextAccessor httpContextAccessor,
        IOptions<JwtOptions> options, ITokenSerializerService tokenSerializerService)
    {
        _usersService = usersService;
        _httpContextAccessor = httpContextAccessor;
        _tokenSerializerService = tokenSerializerService;
        _options = options.Value;
    }

    public async Task<Result<User>> RegisterAsync(CreateUser createdUser, CancellationToken cancellationToken = default)
    {
        var createdUserResult = await _usersService.CreateAsync(createdUser, cancellationToken);
        if (createdUserResult.IsFailure) return createdUserResult;
        await Authenticate(createdUserResult.Value, cancellationToken);
        return createdUserResult;
    }

    public Task<Result<User>> LoginAsync(LoginUser loginUser, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private async Task Authenticate(User user, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        if (_httpContextAccessor.HttpContext is null) return;
        var response = _httpContextAccessor.HttpContext.Response;
        var accessToken = new Token(
            Guid.NewGuid(),
            user.Email.Value,
            now,
            now.AddMinutes(_options.AccessTokenLifetimeMinutes)
        );
        var refreshToken = new Token(
            Guid.NewGuid(),
            user.Email.Value,
            now,
            now.AddDays(_options.RefreshTokenLifetimeDays)
        );
        var accessString = await _tokenSerializerService.SerializeTokenAsync(accessToken, cancellationToken);
        var refreshString = await _tokenSerializerService.SerializeTokenAsync(refreshToken, cancellationToken);
        AddAccessTokenToResponse(response, accessString);
        AddRefreshTokenToResponse(response, refreshString, refreshToken.ExpiresAt - refreshToken.ExpiresAt);
    }

    private static void AddAccessTokenToResponse(HttpResponse response, string accessToken)
    {
        response.Headers[Authorization] = $"{Bearer} {accessToken}";
    }

    private static void AddRefreshTokenToResponse(HttpResponse response, string refreshToken, TimeSpan maxAge)
    {
        response.Cookies.Append(CookieName, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Domain = null,
            Path = "/",
            MaxAge = maxAge
        });
    }
}