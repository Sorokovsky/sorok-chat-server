using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace SorokChatServer.Logic.Models;

public partial class Email : ValueObject
{
    public const int MAX_LENGTH = 20;

    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email> Create(string email)
    {
        if (IsValidEmail(email))
        {
            return Result.Success(new Email(email));
        }

        return Result.Failure<Email>("Не коректна електронна адреса");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || email.Length > MAX_LENGTH) return false;
        return EmailRegex().IsMatch(email);
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, "uk-UA")]
    private static partial Regex EmailRegex();
}