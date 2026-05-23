using SharpChat.Core.DataContracts;
using SharpChat.Core.Models;

namespace SharpChat.Server.Services;

public class ChatsInMemoryManager : IChatsManager
{
    private readonly List<Chat> _chats =
        [
            new Chat(){ Id = 0, Title = "Anna", LastMessage = new Message() { Text = "Hello" }  },
            new Chat(){ Id = 1, Title = "Alex", LastMessage = new Message() { Text = "Hi" }  },
            new Chat(){ Id = 2, Title = "Sergei", LastMessage = new Message() { Text = "How are you?" }  },
        ];

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
        return _chats;
    }

    public Chat? GetById(int chatId)
    {
        return _chats.FirstOrDefault(chat => chat.Id == chatId);
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
