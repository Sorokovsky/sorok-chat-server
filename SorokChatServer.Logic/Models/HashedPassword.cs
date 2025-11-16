using CSharpFunctionalExtensions;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Logic.Models;

public sealed class HashedPassword : ValueObject
{
    public const int MAX_PASSWORD_LENGTH = 100;
    public const int MIN_PASSWORD_LENGTH = 8;
    
    public string Value { get; }

    private HashedPassword(string value)
    {
        Value = value;
    }

    public static async Task<Result<HashedPassword>> Create(string plainPassword, IPasswordHasher passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(plainPassword) || plainPassword.Length < MIN_PASSWORD_LENGTH ||
            plainPassword.Length > MAX_PASSWORD_LENGTH)
        {
            return Result.Failure<HashedPassword>($"Пароль має бути довжиною не менше {MIN_PASSWORD_LENGTH} і не більше {MAX_PASSWORD_LENGTH} символів");
        }
        var hashed = await passwordHasher.HashAsync(plainPassword);
        return Result.Success(new HashedPassword(hashed));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}