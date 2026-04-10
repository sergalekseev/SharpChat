namespace SharpChat.Core.Services;

public class AuthService : IAuthService
{
    public Task<bool> LoginAsync(string username, string password)
    {
        return Task.FromResult(username == "Test" && password == "Test");
    }
}
