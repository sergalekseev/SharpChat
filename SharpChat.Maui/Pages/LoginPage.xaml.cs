using SharpChat.Core.ViewModels;

namespace SharpChat.Maui.Pages;

public partial class LoginPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}