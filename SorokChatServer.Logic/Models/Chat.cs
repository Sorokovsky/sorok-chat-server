using CSharpFunctionalExtensions;

namespace SorokChatServer.Logic.Models;

public class Chat : Base
{
    private readonly List<User> _members;
    private readonly List<Message> _messages;

    private Chat(Title title, Description description, IEnumerable<User> members, IEnumerable<Message> messages)
        : base(0, DateTime.UtcNow, DateTime.UtcNow)
    {
        Title = title;
        Description = description;
        _members = members.ToList();
        _messages = messages.ToList();
    }

    public Title Title { get; }

    public IReadOnlyList<User> Members => _members;
    public IReadOnlyList<Message> Messages => _messages;

    public Description Description { get; }

    public void AddMember(User user)
    {
        if (_members.Contains(user) == false) _members.Add(user);
    }

    public void RemoveMember(User user)
    {
        _members.Remove(user);
    }

    public void AddMessage(Message message)
    {
        if (_messages.Contains(message) == false) _messages.Add(message);
    }

    public virtual void RemoveMessage(Message message)
    {
        _messages.Remove(message);
    }

    public static Result<Chat> Create(string title, string description)
    {
        var titleResult = Title.Create(title);
        var descriptionResult = Description.Create(description);
        if (titleResult.IsFailure) return Result.Failure<Chat>(titleResult.Error);
        if (descriptionResult.IsFailure) return Result.Failure<Chat>(descriptionResult.Error);
        return Result.Success(new Chat(titleResult.Value, descriptionResult.Value, [], []));
    }
}