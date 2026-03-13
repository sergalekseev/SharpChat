using SharpChat.Core.ViewModels;
using System.Windows.Controls;

namespace SharpChat.Wpf.Pages
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        LoginViewModel _vm;

        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = _vm = viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _vm.Password = passwordBox.Password;
            }
        }
    }
}
