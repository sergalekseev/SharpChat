using SharpChat.Core.DataContracts;
using SharpChat.Core.Models;
using System.Security.Claims;

namespace SharpChat.Server.Services;

public interface IChatsManager
{
    IEnumerable<Chat> GetAll();
    IEnumerable<Chat> GetUserChats(ClaimsPrincipal claims);
    Chat? GetById(int chatId);
    Chat? CreateChat(ChatCreateDto newChat);
    Chat? UpdateChat(ChatUpdateDto chatToUpdate);
    Chat DeleteChat(int chatId);
}
