using CSharpFunctionalExtensions;

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

    public Result<User> Create(
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
}