using SharpChat.Core.ViewModels;
using System.Windows.Controls;

namespace SharpChat.Wpf.Pages
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        public ChatPage(ChatViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
