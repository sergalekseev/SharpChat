using SharpChat.Core.DataContracts;
using SharpChat.Core.Models;

namespace SharpChat.Server.Services;

public class ChatsInMemoryManager : IChatsManager
{
    private readonly IMessagesManager _messagesManager;

    public ChatsInMemoryManager(IMessagesManager messagesManager)
    {
        _messagesManager = messagesManager;
    }

    private readonly List<Chat> _chats = MockedDataGenerator.Chats.ToList();

    public Chat? CreateChat(ChatCreateDto newChat)
    {
        if (string.IsNullOrWhiteSpace(newChat.Title))
        {
            throw new InvalidDataException("Title must be valuable");
        }

        var newId = _chats.Max(chat => chat.Id) + 1;

        var chatToCreate = new Chat()
        {
            Id = newId,
            Title = newChat.Title
        };

        _chats.Add(chatToCreate);

        return _chats.FirstOrDefault(chat => chat.Id == newId);
    }

    public Chat DeleteChat(int chatId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Chat> GetAll()
    {
        return _chats.Select(chat => new Chat()
        {
            Id = chat.Id,
            Title = chat.Title,
            LastMessage = _messagesManager.GetLast(chat.Id)
        });
    }

    public Chat? GetById(int chatId)
    {
        var originalChat = _chats.FirstOrDefault(chat => chat.Id == chatId);

        if (originalChat == null) return null;

        return new Chat()
        {
            Id = originalChat.Id,
            Title = originalChat.Title,
            LastMessage = _messagesManager.GetLast(originalChat.Id)
        };
    }

    public Chat? UpdateChat(ChatUpdateDto chatUpdateDto)
    {
        if (string.IsNullOrWhiteSpace(chatUpdateDto.Title))
        {
            throw new InvalidDataException("Title must be valuable");
        }

        var chatToUpdate = _chats.FirstOrDefault(chat => chat.Id == chatUpdateDto.Id);

        if (chatToUpdate is null)
        {
            throw new InvalidDataException($"Chat with id '{chatUpdateDto.Id}' not found");
        }

        chatToUpdate.Title = chatUpdateDto.Title;

        return _chats.FirstOrDefault(chat => chat.Id == chatUpdateDto.Id);
    }
}
