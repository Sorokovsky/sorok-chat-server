using System.Net;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace SorokChatServer.Domain.Models;

public partial class Email : ValueObject
{
    private const string EmailPattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
    private const string EmptyEmailError = "Електронна адреса не може бути порожнім рядком.";
    private const string EmailPatternError = "Не коректний формат електронної адреси.";
    private static string MaxLengthError => $"Електронна адреса не може перевищувати {MaxLength} символів.";
    public const int MaxLength = 100;
    
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email, Error> Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return new Error(EmptyEmailError, HttpStatusCode.BadRequest);
        }

        if (email.Length > MaxLength)
        {
            return new Error(MaxLengthError, HttpStatusCode.BadRequest);
        }

        return EmailRegex().IsMatch(email) is false 
            ? new Error(EmailPatternError, HttpStatusCode.BadRequest)
            : new Email(email);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    [GeneratedRegex(EmailPattern)]
    private static partial Regex EmailRegex();
}