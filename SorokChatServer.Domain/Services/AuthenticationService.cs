using System.Net;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using SorokChatServer.Domain.Contracts;
using SorokChatServer.Domain.Contracts.User;
using SorokChatServer.Domain.Models;

namespace SorokChatServer.Domain.Services;

public class AuthenticationService : IAuthenticationService
{
    private const string ResponseIsNullError = "Невідома помилка. Спробуйте ще.";
    private const string CredentialsError = "Електронна адреса або пароль не коретні.";
    private const string UserExistsError = "Користувач з такою еклектронною вже існує.";
    
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUsersService _usersService;
    private readonly IAccessTokenStorage _accessTokenStorage;
    private readonly IRefreshTokenStorage _refreshTokenStorage;
    private readonly IPasswordHasherService _passwordHasherService;

    public AuthenticationService(
        IHttpContextAccessor contextAccessor, 
        IUsersService usersService, 
        IAccessTokenStorage accessTokenStorage, 
        IRefreshTokenStorage refreshTokenStorage, 
        IPasswordHasherService passwordHasherService
        )
    {
        _contextAccessor = contextAccessor;
        _usersService = usersService;
        _accessTokenStorage = accessTokenStorage;
        _refreshTokenStorage = refreshTokenStorage;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<Result<User, Error>> RegisterAsync(
        NewUser newUser, 
        CancellationToken cancellationToken = default
        )
    {
        var candidateResult = await _usersService.GetByEmailAsync(newUser.Email, cancellationToken);
        if (candidateResult.IsSuccess) return new Error(UserExistsError, HttpStatusCode.BadRequest);
        var createdUserResult = await _usersService.CreateAsync(newUser, cancellationToken);
        if (createdUserResult.IsFailure) return createdUserResult.Error;
        var response = _contextAccessor.HttpContext.Response;
        if (response is null)
        {
            await _usersService.DeleteAsync(createdUserResult.Value.Id, cancellationToken);
            return new Error(ResponseIsNullError, HttpStatusCode.InternalServerError);
        }
        Authenticate(createdUserResult.Value, response);
        return createdUserResult.Value;
    }

    public async Task<Result<User, Error>> LoginAsync(
        LoginUser loginUser, 
        CancellationToken cancellationToken = default
        )
    {
        var error = new Error(CredentialsError, HttpStatusCode.BadRequest);
        var (_, isFailure, user) = await _usersService.GetByEmailAsync(loginUser.Email, cancellationToken);
        if (isFailure) return error;
        var isPasswordCorrect = _passwordHasherService.Verify(user.HashedPassword.Value, loginUser.Password);
        if (isPasswordCorrect is false) return error;
        var response = _contextAccessor.HttpContext.Response;
        if (response is null) return new Error(ResponseIsNullError, HttpStatusCode.InternalServerError);
        Authenticate(user, response);
        return user;
    }

    public Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        var response = _contextAccessor.HttpContext.Response;
        if (response is null) return Task.CompletedTask;
        _accessTokenStorage.ClearToken(response);
        _refreshTokenStorage.ClearToken(response);
        return Task.CompletedTask;
    }

    private void Authenticate(User user, HttpResponse response)
    {
        var email = user.Email.Value;
        var now = DateTime.UtcNow;
        var accessToken = new Token(Guid.NewGuid(), email, now, now.AddMinutes(15));
        var refreshToken = new Token(Guid.NewGuid(), email, now, now.AddDays(7));
        _accessTokenStorage.SetToken(accessToken, response);
        _refreshTokenStorage.SetToken(refreshToken, response);
    }
}