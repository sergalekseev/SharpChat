using SharpChat.Core.Services;

namespace SharpChat.Core.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IAuthService _authService;

    private string _username = string.Empty;
    private string _password = string.Empty;
    private bool _isErrorMessageVisible;

    public LoginViewModel(INavigationService navigationService, IAuthService authService)
    {
        _navigationService = navigationService;
        _authService = authService;

        LoginCommand = new AsyncCommand(LoginAsync);
    }

    public string Username
    {
        get => _username;
        set
        {
            SetProperty(ref _username, value);
            IsErrorMessageVisible = false;
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            SetProperty(ref _password, value);
            IsErrorMessageVisible = false;
        }
    }

    public bool IsErrorMessageVisible
    {
        get => _isErrorMessageVisible;
        set => SetProperty(ref _isErrorMessageVisible, value);
    }

    public AsyncCommand LoginCommand { get; }

    private async Task LoginAsync()
    {
        var success = await _authService.LoginAsync(Username, Password);

        if (success)
        {
            await _navigationService.NavigateToAsync(Route.Chat);
        }

        IsErrorMessageVisible = !success;
    }
}
