using CSharpFunctionalExtensions;
using SorokChatServer.Logic.Contracts;

namespace SorokChatServer.Logic.Models;

public class Message
{
    private Message(Guid id, DateTime createdAt, DateTime updatedAt, string text, string mac, User author)

    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Text = text;
        Mac = mac;
        Author = author;
    }

    public Guid Id { get; }

    public DateTime CreatedAt { get; }

    public DateTime UpdatedAt { get; }

    public string Text { get; }
    public string Mac { get; }

    public User Author { get; set; }

    public static Result<Message> Create(string text, string mac, User author)
    {
        if (string.IsNullOrWhiteSpace(text)) return Result.Failure<Message>("Текст не може бути пустим.");

        if (string.IsNullOrWhiteSpace(mac)) return Result.Failure<Message>("Повідомлення не підписаною.");

        return Result.Success(new Message(
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                text,
                mac,
                author
            )
        );
    }

    public GetMessage ToGet()
    {
        return new GetMessage(
            Id,
            CreatedAt,
            UpdatedAt,
            Text,
            Mac,
            Author.ToGet()
        );
    }
}