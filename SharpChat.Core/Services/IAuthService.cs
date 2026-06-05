namespace SharpChat.Core.Services;

public interface IAuthService
{
    string AuthToken { get; }

    Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken);
    Task<bool> TryAutoLoginAsync();
    Task LogoutAsync();
}

