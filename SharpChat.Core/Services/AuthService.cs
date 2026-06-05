using Microsoft.Maui.Storage;
using SharpChat.Core.Services.ApiClients;

namespace SharpChat.Core.Services;

public class AuthService : IAuthService
{
    IAuthApiClient _authApiClient;

    public AuthService(IAuthApiClient authApiClient)
    {
        _authApiClient = authApiClient;
    }

    public string AuthToken { get; private set; }

    public async Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken)
    {
        var result = await _authApiClient.LoginAsync(username, password, cancellationToken);

        if (result.Success)
        {
            AuthToken = result.AccessToken;

            await SecureStorage.Default.SetAsync(Consts.StorageAccessTokenKey, result.AccessToken);
            await SecureStorage.Default.SetAsync(Consts.StorageAccessTokenExpiresKey, result.AccessTokenExpiry.ToString());

            return true;
        }

        return false;
    }

    public Task LogoutAsync()
    {
        AuthToken = string.Empty;
        SecureStorage.Default.Remove(Consts.StorageAccessTokenKey);
        return Task.CompletedTask;
    }

    public async Task<bool> TryAutoLoginAsync()
    {
        try
        {
            var token = await SecureStorage.Default.GetAsync(Consts.StorageAccessTokenKey);
            var expiresStr = await SecureStorage.Default.GetAsync(Consts.StorageAccessTokenExpiresKey);

            if (string.IsNullOrWhiteSpace(token))
                return false;

            AuthToken = token;

            if (!string.IsNullOrEmpty(expiresStr) &&
                DateTime.TryParse(expiresStr, out var expiresAt) &&
                expiresAt < DateTime.UtcNow)
            {
                await LogoutAsync();
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
