using SharpChat.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SharpChat.Core.ViewModels;

public class ChatViewModel : BaseViewModel
{
    public ObservableCollection<Message> Messages { get; }
    private string _currentMessageText = string.Empty;
    private User _currentUser;

    public ChatViewModel()
    {
        _currentUser = new User()
        {
            Username = "User1"
        };

        var user2 = new User()
        {
            Username = "User2"
        };

        Messages = new()
        {
            new() { Sender = _currentUser, Text = "Hello" },
            new() { Sender = user2, Text = "World" },
        };

        SubmitCommand = new AsyncCommand(SubmitClicked);
        CleanupCommand = new AsyncCommand(CleanupClicked);
    }

    public string CurrentMessageText
    {
        get => _currentMessageText;
        set => SetProperty(ref _currentMessageText, value);
    }

    public ICommand SubmitCommand { get; }
    public ICommand CleanupCommand { get; }

    private Task SubmitClicked()
    {
        if (string.IsNullOrWhiteSpace(_currentMessageText))
        {
            return Task.CompletedTask;
        }

        Messages.Add(new()
        {
            Sender = _currentUser,
            Text = _currentMessageText,
            Time = DateTime.Now
        });

        CurrentMessageText = string.Empty;

        return Task.CompletedTask;
    }

    private Task CleanupClicked()
    {
        Messages.Clear();
        return Task.CompletedTask;
    }
}
