using SharpChat.Core.DataContracts;
using System.Net.Http.Json;

namespace SharpChat.Core.Services.ApiClients;

public class AuthApiClient : IAuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthResponseDto> LoginAsync(string username, string password, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", new LoginDto() { Username = username, Password = password }, cancellationToken: cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AuthResponseDto>(cancellationToken: cancellationToken) ?? new AuthResponseDto();
        }

        return new AuthResponseDto();
    }
}

