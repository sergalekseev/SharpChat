using SharpChat.Core.DataContracts;
using SharpChat.Core.Models;
using System.Security.Claims;

namespace SharpChat.Server.Services;

public class MessagesInMemoryManager : IMessagesManager
{
    private readonly Dictionary<int, List<Message>?> _messages;

    public MessagesInMemoryManager()
    {
        var chats = MockedDataGenerator.Chats.ToList();

        _messages = chats.ToDictionary(
            chat => chat.Id,
            MockedDataGenerator.GenerateMessagesForChat
        );
    }

    public Message? CreateMessage(ClaimsPrincipal user, MessageCreateDto newMessage)
    {
        if (newMessage is null)
        {
            throw new InvalidDataException("Incorrect input data");
        }

        if (string.IsNullOrWhiteSpace(newMessage.Text))
        {
            throw new InvalidDataException("New message text must be valuable");
        }

        if (!_messages.ContainsKey(newMessage.ChatId))
        {
            throw new InvalidDataException($"Chat with id '{newMessage.ChatId}' not found");
        }

        var currentUser = new User()
        {
            Id = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!),
            Username = user.Identity?.Name ?? throw new InvalidDataException("User identity doesn't exist or corrupted")
        };

        if (_messages[newMessage.ChatId] is null)
        {
            _messages[newMessage.ChatId] = new List<Message>();
        }

        var newMessageId = _messages[newMessage.ChatId]!.Max(m => m.Id) + 1;
        var resultMessage = new Message()
        {
            Id = newMessageId,
            ChatId = newMessage.ChatId,
            Text = newMessage.Text,
            Sender = currentUser,
            Time = DateTime.Now,
        };

        _messages[newMessage.ChatId]!.Add(resultMessage);
        return resultMessage;
    }

    public IEnumerable<Message>? GetAll(int chatId)
    {
        return _messages[chatId];
    }

    public Message? GetLast(int chatId)
    {
        return _messages[chatId]?.MaxBy(message => message.Id);
    }
}
