using CSharpFunctionalExtensions;

namespace SorokChatServer.Logic.Models;

public sealed class Name : ValueObject
{
    public const int MAX_FIRST_NAME_LENGTH = 20;
    public const int MAX_LAST_NAME_LENGTH = 20;
    public const int MAX_MIDDLE_NAME_LENGTH = 20;
    
    public string FirstName { get; }
    public string LastName { get; }
    public string MiddleName { get; }

    private Name(string firstName, string lastName, string middleName)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    public static Result<Name> Create(string firstName, string lastName, string middleName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > MAX_FIRST_NAME_LENGTH)
        {
            return Result.Failure<Name>($"Ім'я має бути не пустим і не довшим ніж {MAX_FIRST_NAME_LENGTH} символів");
        }

        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > MAX_LAST_NAME_LENGTH)
        {
            return Result.Failure<Name>($"Прізвище має бути не пустим і не довшим ніж {MAX_LAST_NAME_LENGTH} символів");
        }

        if (string.IsNullOrWhiteSpace(middleName) || middleName.Length > MAX_MIDDLE_NAME_LENGTH)
        {
            return Result.Failure<Name>($"По батькові має бути не пустим і не довшим ніж {MAX_MIDDLE_NAME_LENGTH} символів");
        }
        return Result.Success(new Name(firstName, lastName, middleName));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return MiddleName;
    }
}