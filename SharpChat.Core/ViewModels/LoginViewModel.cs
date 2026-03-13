using SharpChat.Core.Services;

namespace SharpChat.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IAuthService _authService;

        private string _username;
        private string _password;

        public LoginViewModel(INavigationService navigationService, IAuthService authService)
        {
            _navigationService = navigationService;
            _authService = authService;

            LoginCommand = new AsyncCommand(LoginAsync);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public AsyncCommand LoginCommand { get; }

        private async Task LoginAsync()
        {
            var success = await _authService.LoginAsync(Username, Password);

            if (success)
            {
                await _navigationService.NavigateToAsync(Route.Chat);
            }
        }
    }
}
