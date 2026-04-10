using SharpChat.Core.Models;
using System.Collections.ObjectModel;

namespace SharpChat.Core.ViewModels;

public class ChatViewModel : BaseViewModel
{
    private readonly ObservableCollection<Message> _messages;
    private string _currentMessageText = string.Empty;
    private string _searchText = string.Empty;

    private User _currentUser;

    public ChatViewModel()
    {
        _currentUser = new User()
        {
            Username = "User1"
        };

        СhatParticipant = new User()
        {
            Username = "User2"
        };

        _messages = new()
        {
            new() { Sender = _currentUser, Text = "Hello" },
            new() { Sender = СhatParticipant, Text = "World" },
        };
        FilteredMessages = new(_messages);

        _messages.CollectionChanged += MessagesCollectionChanged;

        SubmitCommand = new AsyncCommand(SubmitClicked, CheckSubmitCanExecute);
        CleanupCommand = new AsyncCommand(CleanupClicked, CheckCleanupCanExecute);
    }

    public ObservableCollection<Message> FilteredMessages { get; }

    public User СhatParticipant { get; }

    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            FilterMessages();
        }
    }

    public bool IsSearchEnabled => _messages.Count > 0;

    public string CurrentMessageText
    {
        get => _currentMessageText;
        set
        {
            SetProperty(ref _currentMessageText, value);
            SubmitCommand.RaiseCanExecuteChanged();
        }
    }

    public AsyncCommand SubmitCommand { get; }
    public AsyncCommand CleanupCommand { get; }

    private Task SubmitClicked()
    {
        if (string.IsNullOrWhiteSpace(_currentMessageText))
        {
            return Task.CompletedTask;
        }

        _messages.Add(new()
        {
            Sender = _currentUser,
            Text = _currentMessageText,
            Time = DateTime.Now
        });

        CurrentMessageText = string.Empty;
        CleanupCommand.RaiseCanExecuteChanged();

        return Task.CompletedTask;
    }

    private bool CheckSubmitCanExecute()
    {
        return !string.IsNullOrWhiteSpace(CurrentMessageText);
    }

    private Task CleanupClicked()
    {
        _messages.Clear();
        CleanupCommand.RaiseCanExecuteChanged();

        return Task.CompletedTask;
    }

    private bool CheckCleanupCanExecute()
    {
        return _messages.Count > 0;
    }

    private void MessagesCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
        {
            FilteredMessages.Clear();
        }
        else
        {
            FilterMessages();
        }

        RaisePropertyChangedEvent(nameof(IsSearchEnabled));
    }

    private void FilterMessages()
    {
        var isSearchTextNotValuable = string.IsNullOrWhiteSpace(SearchText);

        var targetList = _messages.Where(message =>
            isSearchTextNotValuable || message.Text.Contains(SearchText)).ToList();

        for (var i = FilteredMessages.Count - 1; i >= 0; i--)
        {
            if (isSearchTextNotValuable)
            {
                break;
            }

            var item = FilteredMessages[i];
            if (!targetList.Contains(item))
            {
                FilteredMessages.RemoveAt(i);
            }
        }

        for (var i = 0; i < targetList.Count; i++)
        {
            var targetItem = targetList[i];

            if (i >= FilteredMessages.Count || FilteredMessages[i] != targetItem)
            {
                var oldIndex = FilteredMessages.IndexOf(targetItem);

                if (oldIndex >= 0)
                {
                    FilteredMessages.Move(oldIndex, i);
                }
                else
                {
                    FilteredMessages.Insert(i, targetItem);
                }
            }
        }
    }
}
