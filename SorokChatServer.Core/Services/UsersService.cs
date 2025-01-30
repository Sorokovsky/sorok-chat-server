using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using SorokChatServer.Core.Contracts;
using SorokChatServer.Core.Entities;
using SorokChatServer.Core.Interfaces;
using SorokChatServer.Core.Models;

namespace SorokChatServer.Core.Services;

public class UsersService : IUsersService
{
    private readonly IFilesService _filesService;
    private readonly IPasswordService _passwordService;
    private readonly IUsersRepository _repository;

    public UsersService(IPasswordService passwordService, IUsersRepository repository, IFilesService filesService)
    {
        _passwordService = passwordService;
        _repository = repository;
        _filesService = filesService;
    }

    public async Task<Result<User, ApiError>> Create(CreateUserRequest newUser, CancellationToken cancellationToken)
    {
        newUser = newUser with { password = await _passwordService.Encrypt(newUser.password) };
        var userResult = User.Create(newUser);
        if (userResult.IsFailure) return userResult;
        var createdUser = await _repository.Create(userResult.Value, cancellationToken);
        if (createdUser.IsFailure) return createdUser;
        var avatarPath = await UploadAvatarIfExists(newUser.avatar, createdUser.Value.Id, cancellationToken);
        return await _repository.Update(createdUser.Value.Id, new UserEntity { AvatarPath = avatarPath },
            cancellationToken);
    }

    public async Task<Result<User, ApiError>> GetById(long id, CancellationToken cancellationToken)
    {
        return await _repository.GetBy(user => user.Id == id, cancellationToken);
    }

    private async Task<string> UploadAvatarIfExists(IFormFile? avatar, long id, CancellationToken cancellationToken)
    {
        if (avatar is null) return string.Empty;
        var uploadResult = await _filesService.Upload(avatar, id.ToString(), nameof(avatar), true, cancellationToken);
        return uploadResult.IsFailure ? string.Empty : uploadResult.Value;
    }
}