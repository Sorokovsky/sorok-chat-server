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
    private const string InvalidCredentials = "Не вірні авторизаційні данні";
    private readonly IAccessTokenStorage _accessTokenStorage;

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtOptions _options;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenStorage _refreshTokenStorage;
    private readonly IUsersService _usersService;

    public AuthenticationService(
        IUsersService usersService,
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtOptions> options,
        IPasswordHasher passwordHasher,
        IAccessTokenStorage accessTokenStorage,
        IRefreshTokenStorage refreshTokenStorage
    )
    {
        _usersService = usersService;
        _httpContextAccessor = httpContextAccessor;
        _passwordHasher = passwordHasher;
        _accessTokenStorage = accessTokenStorage;
        _refreshTokenStorage = refreshTokenStorage;
        _options = options.Value;
    }

    public async Task<Result<User>> RegisterAsync(CreateUser createdUser, CancellationToken cancellationToken = default)
    {
        var createdUserResult = await _usersService.CreateAsync(createdUser, cancellationToken);
        if (createdUserResult.IsFailure) return createdUserResult;
        await Authenticate(createdUserResult.Value, cancellationToken);
        return createdUserResult;
    }

    public async Task<Result<User>> LoginAsync(LoginUser loginUser, CancellationToken cancellationToken = default)
    {
        var found = await _usersService.GetByEmailAsync(loginUser.Email, cancellationToken);
        if (found.IsFailure) return Result.Failure<User>(InvalidCredentials);
        var user = found.Value;
        var isPasswordValid =
            await _passwordHasher.VerifyAsync(loginUser.Password, user.Password.Value, cancellationToken);
        if (!isPasswordValid) return Result.Failure<User>(InvalidCredentials);
        await Authenticate(user, cancellationToken);
        return Result.Success(user);
    }

    public async Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        if (_httpContextAccessor.HttpContext is null) return;
        var response = _httpContextAccessor.HttpContext.Response;
        await _accessTokenStorage.ClearTokenAsync(response, cancellationToken);
        await _refreshTokenStorage.ClearTokenAsync(response, cancellationToken);
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
        await _accessTokenStorage.SetTokenAsync(accessToken, response, cancellationToken);
        await _refreshTokenStorage.SetTokenAsync(refreshToken, response, cancellationToken);
    }
}