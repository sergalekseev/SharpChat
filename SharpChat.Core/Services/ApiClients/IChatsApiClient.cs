using SharpChat.Core.Models;

namespace SharpChat.Core.Services.ApiClients;

public interface IChatsApiClient
{
    Task<IEnumerable<Chat>> GetAllAsync();
    Task<Chat?> GetByIdAsync(int chatId);
}
