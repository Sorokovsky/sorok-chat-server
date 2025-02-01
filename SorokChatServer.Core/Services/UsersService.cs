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
        var update = new UserEntity { AvatarPath = avatarPath };
        return await _repository.Update(createdUser.Value.Id, User.FromEntity(update), cancellationToken);
    }

    public async Task<Result<User, ApiError>> GetById(long id, CancellationToken cancellationToken)
    {
        return await _repository.GetBy(user => user.Id == id, cancellationToken);
    }

    public async Task<Result<User, ApiError>> Update(long id, UpdateUserRequest user,
        CancellationToken cancellationToken)
    {
        var candidateResult = await GetById(id, cancellationToken);
        var candidate = candidateResult.Value!;
        if (candidateResult.IsFailure) return candidateResult;
        if (string.IsNullOrWhiteSpace(user.password) is false)
            user = user with { password = await _passwordService.Encrypt(user.password) };

        var uploadedAvatar = await UploadAvatarIfExists(user.avatar, id, cancellationToken);
        if (string.IsNullOrWhiteSpace(uploadedAvatar)) uploadedAvatar = candidate.AvatarPath;

        var userResult = User.Create(user, uploadedAvatar);
        if (userResult.IsFailure) return userResult;

        return await _repository.Update(id, userResult.Value, cancellationToken);
    }

    public async Task<Result<User, ApiError>> Delete(long id, CancellationToken cancellationToken)
    {
        var userResult = await GetById(id, cancellationToken);
        if (userResult.IsFailure) return userResult;
        await DeleteAvatarIfExists(userResult.Value.AvatarPath, cancellationToken);
        return await _repository.Delete(id, cancellationToken);
    }

    private async Task<string> UploadAvatarIfExists(IFormFile? avatar, long id, CancellationToken cancellationToken)
    {
        if (avatar is null) return string.Empty;
        var uploadResult = await _filesService.Upload(avatar, id.ToString(), nameof(avatar), true, cancellationToken);
        return uploadResult.IsFailure ? string.Empty : uploadResult.Value;
    }

    private async Task DeleteAvatarIfExists(string avatarPath, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(avatarPath)) return;
        await _filesService.Delete(avatarPath, cancellationToken);
    }
}