using System.Net;
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
    private readonly IRepository<UserEntity> _repository;

    public UsersService(IPasswordService passwordService, IRepository<UserEntity> repository,
        IFilesService filesService)
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
        var createdUser = await _repository.Create(userResult.Value.ToEntity(), cancellationToken);
        if (createdUser.IsFailure) return Result.Failure<User, ApiError>(createdUser.Error);
        var avatarPath = await UploadAvatarIfExists(newUser.avatar, createdUser.Value.Id, cancellationToken);
        var update = new UserEntity { AvatarPath = avatarPath };
        var result = await _repository.Update(x => x.Id == createdUser.Value.Id, update, cancellationToken);
        if (result.IsFailure) return Result.Failure<User, ApiError>(result.Error);
        return Result.Success<User, ApiError>(User.FromEntity(result.Value));
    }

    public async Task<Result<User, ApiError>> GetById(long id, CancellationToken cancellationToken)
    {
        var candidate = await _repository.GetOneBy(user => user.Id == id, cancellationToken);
        if (candidate.IsFailure) return Result.Failure<User, ApiError>(candidate.Error);
        return Result.Success<User, ApiError>(User.FromEntity(candidate.Value));
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

        var result = await _repository.Update(x => x.Id == id, userResult.Value.ToEntity(), cancellationToken);
        if (result.IsFailure) return Result.Failure<User, ApiError>(result.Error);
        return Result.Success<User, ApiError>(User.FromEntity(result.Value));
    }

    public async Task<Result<User, ApiError>> Delete(long id, CancellationToken cancellationToken)
    {
        var userResult = await GetById(id, cancellationToken);
        if (userResult.IsFailure) return userResult;
        await DeleteAvatarIfExists(userResult.Value.AvatarPath, cancellationToken);
        var result = await _repository.Delete(x => x.Id == id, cancellationToken);
        if (result.IsFailure) return Result.Failure<User, ApiError>(result.Error);
        return Result.Success<User, ApiError>(User.FromEntity(result.Value));
    }

    public async Task<Result<User, ApiError>> DeleteAvatar(long id, CancellationToken cancellationToken)
    {
        var candidateResult = await GetById(id, cancellationToken);
        if (candidateResult.IsFailure) return candidateResult;
        var deletingResult = await DeleteAvatarIfExists(candidateResult.Value.AvatarPath, cancellationToken);
        if (deletingResult.IsFailure) return Result.Failure<User, ApiError>(deletingResult.Error);

        var entity = new UserEntity
        {
            AvatarPath = string.Empty
        };
        var result = await _repository.Update(x => x.Id == id, entity, cancellationToken);
        if (result.IsFailure) return Result.Failure<User, ApiError>(result.Error);

        return Result.Success<User, ApiError>(User.FromEntity(result.Value));
    }

    private async Task<string> UploadAvatarIfExists(IFormFile? avatar, long id, CancellationToken cancellationToken)
    {
        if (avatar is null) return string.Empty;
        var uploadResult = await _filesService.Upload(avatar, id.ToString(), nameof(avatar), true, cancellationToken);
        return uploadResult.IsFailure ? string.Empty : uploadResult.Value;
    }

    private async Task<Result<bool, ApiError>> DeleteAvatarIfExists(string avatarPath,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(avatarPath))
            return Result.Failure<bool, ApiError>(new ApiError("Avatar undefined.", HttpStatusCode.BadRequest));

        return await _filesService.Delete(avatarPath, cancellationToken);
    }
}