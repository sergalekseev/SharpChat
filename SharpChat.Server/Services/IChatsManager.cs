using SharpChat.Core.DataContracts;
using SharpChat.Core.Models;

namespace SharpChat.Server.Services;

public interface IChatsManager
{
    IEnumerable<Chat> GetAll();
    Chat? GetById(int chatId);
    Chat? CreateChat(ChatCreateDto newChat);
    Chat? UpdateChat(ChatUpdateDto chatToUpdate);
    Chat DeleteChat(int chatId);
}
