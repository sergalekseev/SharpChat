using SharpChat.Core.DataContracts;
using SharpChat.Core.Models;
using System.Net.Http.Json;

namespace SharpChat.Core.Services.ApiClients;

public class MessagesApiClient : IMessagesApiClient
{
    private readonly HttpClient _httpClient;

    public MessagesApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Message>?> GetAllAsync(int chatId)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<Message>?>($"api/messages/list/{chatId}");
        return result ?? Enumerable.Empty<Message>();
    }

    public async Task<Message?> SendMessage(MessageCreateDto message)
    {
        var response = await _httpClient.PostAsJsonAsync("api/messages", message, CancellationToken.None);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Message>();
        }

        return null;
    }
}

