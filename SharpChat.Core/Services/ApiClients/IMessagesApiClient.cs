using SharpChat.Core.Models;

namespace SharpChat.Core.Services.ApiClients;

public interface IMessagesApiClient
{
    Task<IEnumerable<Message>?> GetAllAsync(int chatId);
}
