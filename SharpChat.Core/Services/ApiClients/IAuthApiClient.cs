using SharpChat.Core.DataContracts;

namespace SharpChat.Core.Services.ApiClients;

public interface IAuthApiClient
{
    Task<AuthResponseDto> LoginAsync(string username, string password, CancellationToken cancellationToken);
}

