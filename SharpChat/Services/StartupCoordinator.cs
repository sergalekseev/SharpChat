namespace SharpChat.Core.Services
{
    public class StartupCoordinator : IStartupCoordinator
    {
        private readonly INavigationService _navigationService;

        public StartupCoordinator(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task StartAsync()
        {
            // assume always needs to login
            await _navigationService.NavigateToAsync(Route.Login);
        }
    }
}
