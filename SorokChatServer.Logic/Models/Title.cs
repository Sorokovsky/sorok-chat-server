using CSharpFunctionalExtensions;

namespace SorokChatServer.Logic.Models;

public class Title : ValueObject
{
    public const int MAX_LENGTH = 100;

    private Title(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Title> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > MAX_LENGTH)
            return Result.Failure<Title>($"Назва чату не може бути пустою чи більшою ніж {MAX_LENGTH} символів.");

        return Result.Success(new Title(title));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}