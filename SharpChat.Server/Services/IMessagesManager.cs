using SharpChat.Core.Models;

namespace SharpChat.Server.Services;

public interface IMessagesManager
{
    IEnumerable<Message>? GetAll(int chatId);
    Message? GetLast(int chatId);
}
