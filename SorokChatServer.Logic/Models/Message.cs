using CSharpFunctionalExtensions;
using SorokChatServer.Postgres.Entities;

namespace SorokChatServer.Logic.Models;

public class Message : Base
{
    private Message(long id, DateTime createdAt, DateTime updatedAt, string text, string mac, User author)
        : base(id, createdAt, updatedAt)
    {
        Text = text;
        Mac = mac;
        Author = author;
    }

    public string Text { get; }
    public string Mac { get; }

    public User Author { get; set; }

    public static Result<Message> Create(string text, string mac, User author)
    {
        if (string.IsNullOrWhiteSpace(text)) return Result.Failure<Message>("Текст не може бути пустим.");

        if (string.IsNullOrWhiteSpace(mac)) return Result.Failure<Message>("Повідомлення не підписаною.");

        return Result.Success(new Message(
                0,
                DateTime.UtcNow,
                DateTime.UtcNow,
                text,
                mac,
                author
            )
        );
    }

    public static Message FromEntity(MessageEntity entity)
    {
        return new Message(
            entity.Id,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.Text, entity.Mac,
            User.FromEntity(entity.Author)
        );
    }
}