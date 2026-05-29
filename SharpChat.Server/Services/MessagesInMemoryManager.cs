using SharpChat.Core.Models;

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


    public IEnumerable<Message>? GetAll(int chatId)
    {
        return _messages[chatId];
    }

    public Message? GetLast(int chatId)
    {
        return _messages[chatId]?.MaxBy(message => message.Id);
    }
}
