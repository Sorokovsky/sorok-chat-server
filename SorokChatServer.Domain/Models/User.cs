using CSharpFunctionalExtensions;
using SorokChatServer.Domain.Contracts.User;

namespace SorokChatServer.Domain.Models;

public class User : Base
{
    public Email Email { get; }
    
    public HashedPassword HashedPassword { get; }
    
    public FullName FullName { get; }
    
    private User(
        long id, 
        DateTime createdAt, 
        DateTime updatedAt,
        Email email,
        HashedPassword hashedPassword,
        FullName fullName
        ) : base(id, createdAt, updatedAt)
    {
        Email = email;
        HashedPassword = hashedPassword;
        FullName = fullName;
    }

    public static Result<User, Error> Create(
        string email, 
        HashedPassword password,
        string firstName,
        string lastName,
        string middleName
        )
    {
        var emailResult = Email.Create(email);
        if (emailResult.IsFailure) return emailResult.Error;
        var now = DateTime.UtcNow;
        var fullNameResult = FullName.Create(firstName, lastName, middleName);
        if (fullNameResult.IsFailure) return fullNameResult.Error;
        return new User(
            0,
            now,
            now,
            emailResult.Value,
            password,
            fullNameResult.Value
        );
    }
}