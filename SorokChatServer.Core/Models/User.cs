using CSharpFunctionalExtensions;
using SorokChatServer.Core.Contracts;
using SorokChatServer.Core.Entities;

namespace SorokChatServer.Core.Models;

public class User : Base
{
    private User(
        long id,
        DateTime createdAt,
        DateTime updatedAt,
        Email email,
        string password,
        string surname,
        string name,
        string middleName,
        string avatarPath
    ) : base(id, createdAt, updatedAt)
    {
        Email = email;
        Password = password;
        Surname = surname;
        Name = name;
        MiddleName = middleName;
        AvatarPath = avatarPath;
    }

    public Email Email { get; }

    public string Password { get; }

    public string Surname { get; }

    public string Name { get; }

    public string MiddleName { get; }

    public string AvatarPath { get; }

    public static Result<User, ApiError> Create(
        long id,
        DateTime createdAt,
        DateTime updatedAt,
        string email,
        string password,
        string surname,
        string name,
        string middleName,
        string avatarPath
    )
    {
        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
        {
            return Result.Failure<User, ApiError>(emailResult.Error);
        }

        return Result.Success<User, ApiError>(
            new User(
                id,
                createdAt,
                updatedAt,
                emailResult.Value,
                password,
                surname,
                name,
                middleName,
                avatarPath
            ));
    }

    public static User FromEntity(UserEntity entity)
    {
        return new User(
            entity.Id,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.Email,
            entity.Password,
            entity.Surname,
            entity.Name,
            entity.MiddleName,
            entity.AvatarPath
        );
    }

    public static Result<User, ApiError> Create(CreateUserRequest newUser)
    {
        return Create(
            0,
            DateTime.UtcNow,
            DateTime.UtcNow,
            newUser.email,
            newUser.password,
            newUser.surname ?? string.Empty,
            newUser.name ?? string.Empty,
            newUser.middleName ?? string.Empty,
            string.Empty
        );
    }

    public static Result<User, ApiError> Create(UpdateUserRequest updatedUser, string? avatarPath)
    {
        Email email = null;
        if (string.IsNullOrWhiteSpace(updatedUser.email) is false)
        {
            var emailResult = Email.Create(updatedUser.email);
            if (emailResult.IsFailure) return Result.Failure<User, ApiError>(emailResult.Error);
            email = emailResult.Value;
        }

        return Result.Success<User, ApiError>(new User(
            0,
            DateTime.UtcNow,
            DateTime.UtcNow,
            email,
            updatedUser.password,
            updatedUser.surname,
            updatedUser.name,
            updatedUser.middleName,
            avatarPath
        ));
    }

    public UserEntity ToEntity()
    {
        return new UserEntity()
        {
            Id = Id,
            Email = Email,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            AvatarPath = AvatarPath,
            Password = Password,
            Surname = Surname,
            Name = Name,
            MiddleName = MiddleName
        };
    }

    public UserResponse ToResponse()
    {
        return new UserResponse(
            Id,
            CreatedAt,
            UpdatedAt,
            Email.Value,
            Surname,
            Name,
            MiddleName,
            AvatarPath
        );
    }
}