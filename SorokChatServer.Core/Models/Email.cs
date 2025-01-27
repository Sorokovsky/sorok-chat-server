using System.Net;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace SorokChatServer.Core.Models;

public class Email : ValueObject
{
    private static readonly Regex EmailRegex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

    private Email()
    {
    }

    private Email(string email)
    {
        Value = email;
    }

    public string Value { get; }

    public static Result<Email, ApiError> Create(string email)
    {
        var math = EmailRegex.Match(email);
        if (math.Success) return Result.Success<Email, ApiError>(new Email(email));
        return Result.Failure<Email, ApiError>(new ApiError("Email is not correct.", HttpStatusCode.BadRequest));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}