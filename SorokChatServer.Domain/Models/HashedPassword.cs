using System.Net;
using CSharpFunctionalExtensions;
using SorokChatServer.Domain.Services;

namespace SorokChatServer.Domain.Models;

public class HashedPassword : ValueObject
{
    public const int MaxLength = 200;
    public const int MinLength = 8;

    private const string PasswordEmptyError = "Пароль не може бути пустим.";
    
    private static string PasswordMaxLengthError => $"Пароль не може перевищувати {MaxLength} символів.";
    private static string PasswordMinLengthError => $"Пароль не може мати меньше ніж {MinLength} символів.";
    
    public string Value { get; }
    
    private HashedPassword(string value) => Value = value;

    public static Result<HashedPassword, Error> Create(
        string? password, 
        IPasswordHasherService passwordHasherService
        )
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return Result.Failure<HashedPassword, Error>(new Error(PasswordEmptyError, HttpStatusCode.BadRequest));
        }

        switch (password.Length)
        {
            case < MinLength:
                return Result.Failure<HashedPassword, Error>(new Error(PasswordMinLengthError, HttpStatusCode.BadRequest));
            case > MaxLength:
                Result.Failure<HashedPassword, Error>(new Error(PasswordMaxLengthError, HttpStatusCode.BadRequest));
                break;
        }

        var hashed = passwordHasherService.Hash(password);
        return Result.Success<HashedPassword, Error>(new HashedPassword(hashed));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}