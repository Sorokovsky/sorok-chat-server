using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using SorokChatServer.Core.Contracts;
using SorokChatServer.Core.Entities;
using SorokChatServer.Core.Interfaces;
using SorokChatServer.Core.Models;

namespace SorokChatServer.Core.Services;

public class UsersService : IUsersService
{
    private readonly IPasswordService _passwordService;
    private readonly IUsersRepository _repository;
    private readonly IFilesService _filesService;

    public UsersService(IPasswordService passwordService, IUsersRepository repository, IFilesService filesService)
    {
        _passwordService = passwordService;
        _repository = repository;
        _filesService = filesService;
    }

    public async Task<Result<User, ApiError>> Create(CreateUserRequest newUser, CancellationToken cancellationToken)
    {
        var hashedPassword = await _passwordService.Encrypt(newUser.password);
        newUser = newUser with { password = hashedPassword };
        var userResult = User.Create(newUser);
        if (userResult.IsFailure) return userResult.Error;
        var createdUser = await _repository.Create(userResult.Value, cancellationToken);
        if (createdUser.IsFailure) return createdUser.Error;
        var avatarPath = await UploadAvatarIfExists(newUser.avatar, createdUser.Value.Id, cancellationToken);
        var updated = await _repository.Update(createdUser.Value.Id, new UserEntity
        {
            AvatarPath = avatarPath
        }, cancellationToken);
        return updated;
    }

    public Task<Result<User, ApiError>> GetById(long id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task<string> UploadAvatarIfExists(IFormFile? avatar, long id, CancellationToken cancellationToken)
    {
        if (avatar is null) return string.Empty;
        var folder = $"{id}";
        var uploadResult = await _filesService.Upload(avatar, folder, nameof(avatar), true, cancellationToken);
        if (uploadResult.IsFailure) return string.Empty;
        return uploadResult.Value;
    }
}