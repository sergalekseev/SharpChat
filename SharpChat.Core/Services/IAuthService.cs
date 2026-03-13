namespace SharpChat.Core.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
    }
}
