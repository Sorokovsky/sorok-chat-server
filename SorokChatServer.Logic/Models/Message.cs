using CSharpFunctionalExtensions;

namespace SorokChatServer.Logic.Models;

public class Message : Base
{
    private Message(string text, string mac, User author)
        : base(0, DateTime.UtcNow, DateTime.UtcNow)
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

        return Result.Success(new Message(text, mac, author));
    }
}