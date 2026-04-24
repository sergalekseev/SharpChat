using SharpChat.Core.ViewModels;

namespace SharpChat.Maui.Pages;

public partial class ChatPage
{
    public ChatPage(ChatViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

    }
}