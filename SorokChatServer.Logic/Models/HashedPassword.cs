using CSharpFunctionalExtensions;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Logic.Models;

public class HashedPassword : ValueObject
{
    public const int MaxPasswordLength = 100;
    public const int MinPasswordLength = 8;

    private HashedPassword(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static async Task<Result<HashedPassword>> Create(string? plainPassword, IPasswordHasher passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(plainPassword) || plainPassword.Length < MinPasswordLength ||
            plainPassword.Length > MaxPasswordLength)
        {
            return Result.Failure<HashedPassword>(
                $"Пароль має бути довжиною не менше {MinPasswordLength} і не більше {MaxPasswordLength} символів");
        }

        var hashed = await passwordHasher.HashAsync(plainPassword);
        return Result.Success(new HashedPassword(hashed));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}