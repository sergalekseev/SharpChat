namespace SharpChat.Core.Services
{
    public interface INavigationService
    {
        Task NavigateToAsync(Route route);
        Task GoBackAsync();
    }
}
