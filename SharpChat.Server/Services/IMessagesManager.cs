using SharpChat.Core.DataContracts;
using SharpChat.Core.Models;
using System.Security.Claims;

namespace SharpChat.Server.Services;

public interface IMessagesManager
{
    IEnumerable<Message>? GetAll(int chatId);
    Message? GetLast(int chatId);
    Message? CreateMessage(ClaimsPrincipal user, MessageCreateDto newMessage);
}
