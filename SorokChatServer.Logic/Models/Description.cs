using CSharpFunctionalExtensions;

namespace SorokChatServer.Logic.Models;

public class Description : ValueObject
{
    public const int MAX_LENGTH = 200;

    private Description(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Description> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description) || description.Length > MAX_LENGTH)
            return Result.Failure<Description>($"Опис чату не може бути пустим чи більшим ніж {MAX_LENGTH} символів.");

        return Result.Success(new Description(description));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}