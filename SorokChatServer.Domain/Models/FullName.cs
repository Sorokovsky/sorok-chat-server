using System.Net;
using CSharpFunctionalExtensions;

namespace SorokChatServer.Domain.Models;

public class FullName : ValueObject
{
    public const int MaxLength = 60;

    private static string FirstNameEmptyError => EmptyError("Імʼя");
    private static string LastNameEmptyError => EmptyError("Прізвище");
    private static string MiddleNameEmptyError => EmptyError("По батькові");
    private static string MaxLengthFirstNameError => MaxLengthError("Імʼя");
    private static string MaxLengthLastNameError => MaxLengthError("Прізвище");
    private static string MaxLengthMiddleNameError => MaxLengthError("По батькові");
    
    public string FirstName { get; }
    
    public string LastName { get; }
    
    public string MiddleName { get; }

    private FullName(string firstName, string lastName, string middleName)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    public static Result<FullName, Error> Create(
        string firstName,
        string lastName,
        string middleName
    )
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<FullName, Error>(new Error(FirstNameEmptyError, HttpStatusCode.BadRequest));
        }

        if (firstName.Length > MaxLength)
        {
            return Result.Failure<FullName, Error>(new Error(MaxLengthFirstNameError, HttpStatusCode.BadRequest));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<FullName, Error>(new Error(LastNameEmptyError, HttpStatusCode.BadRequest));
        }

        if (lastName.Length > MaxLength)
        {
            return Result.Failure<FullName, Error>(new Error(MaxLengthLastNameError, HttpStatusCode.BadRequest));
        }

        if (string.IsNullOrWhiteSpace(middleName))
        {
            return Result.Failure<FullName, Error>(new Error(MiddleNameEmptyError, HttpStatusCode.BadRequest));
        }

        if (middleName.Length > MaxLength)
        {
            return Result.Failure<FullName, Error>(new Error(MaxLengthMiddleNameError, HttpStatusCode.BadRequest));
        }

        return Result.Success<FullName, Error>(new FullName(firstName, lastName, middleName));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return MiddleName;
    }
    
    private static string EmptyError(string name)
    {
        return $"{name} не може бути пустим.";
    }

    private static string MaxLengthError(string name, int maxLength = MaxLength)
    {
        return $"{name} не може перевищувати {maxLength} символів.";
    }
}