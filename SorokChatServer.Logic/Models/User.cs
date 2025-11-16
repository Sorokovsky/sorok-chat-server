using CSharpFunctionalExtensions;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Entities;

namespace SorokChatServer.Logic.Models;

public class User : Base
{
    private User(
        long id,
        DateTime createdAt,
        DateTime updatedAt,
        Name name,
        Email email,
        HashedPassword hashedPassword
    ) : base(id, createdAt, updatedAt)
    {
        Name = name;
        Email = email;
        Password = hashedPassword;
        MacSecret = Guid.NewGuid().ToString();
    }

    public Name Name { get; }

    public Email Email { get; }

    public HashedPassword Password { get; }

    public string MacSecret { get; }

    public static Result<User> Create(
        string firstName,
        string lastName,
        string middleName,
        string email,
        HashedPassword hashedPassword
    )
    {
        var nameResult = Name.Create(firstName, lastName, middleName);
        var emailResult = Email.Create(email);
        if (nameResult.IsFailure)
        {
            return Result.Failure<User>(nameResult.Error);
        }

        if (emailResult.IsFailure)
        {
            return Result.Failure<User>(emailResult.Error);
        }

        return Result.Success(new User(
                0,
                DateTime.UtcNow,
                DateTime.UtcNow,
                nameResult.Value,
                emailResult.Value,
                hashedPassword
            )
        );
    }

    public static User FromEntity(UserEntity entity)
    {
        return new User(
            entity.Id,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.Name,
            entity.Email,
            entity.Password
        );
    }

    public UserEntity ToEntity()
    {
        return new UserEntity(
            Id,
            CreatedAt,
            UpdatedAt,
            Name,
            Email,
            Password,
            MacSecret
        );
    }

    public GetUser ToGet()
    {
        return new GetUser(Id, CreatedAt, UpdatedAt, Email.Value, Name.FirstName, Name.LastName, Name.MiddleName,
            MacSecret);
    }
}