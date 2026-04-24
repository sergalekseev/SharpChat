using SharpChat.Core.Models;
using System.Net.Http.Json;

namespace SharpChat.Core.Services.ApiClients;

public class ChatsApiClient : IChatsApiClient
{
    private readonly HttpClient _httpClient;

    public ChatsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Chat>> GetAllAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<Chat>>("api/chats/list");
        return result ?? Enumerable.Empty<Chat>();
    }

    public async Task<Chat?> GetByIdAsync(int chatId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Chat>($"api/chats/{chatId}");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
