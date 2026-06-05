using Microsoft.Maui.ApplicationModel;

namespace SharpChat.Core.Services
{
    public class StartupCoordinator : IStartupCoordinator
    {
        private readonly INavigationService _navigationService;
        private readonly IAuthService _authService;

        public StartupCoordinator(INavigationService navigationService, IAuthService authService)
        {
            _navigationService = navigationService;
            _authService = authService;
        }

        public async Task StartAsync()
        {
            var isLoggedIn = await _authService.TryAutoLoginAsync();

            if (isLoggedIn)
            {
                await MainThread.InvokeOnMainThreadAsync(async () => await _navigationService.NavigateToAsync(Route.Chat));
            }
            else
            {
                await MainThread.InvokeOnMainThreadAsync(async () => await _navigationService.NavigateToAsync(Route.Login));
            }
        }
    }
}
