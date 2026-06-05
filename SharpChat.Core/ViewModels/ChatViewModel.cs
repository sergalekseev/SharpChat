using SharpChat.Core.Models;
using SharpChat.Core.Services;
using SharpChat.Core.Services.ApiClients;
using System.Collections.ObjectModel;

namespace SharpChat.Core.ViewModels;

public class ChatViewModel : BaseViewModel
{
    IChatsApiClient _chatsApiClient;
    IMessagesApiClient _messagesApiClient;
    IMainThread _mainThread;
    IChatRealtimeService _chatRealtimeService;

    private ObservableCollection<Message> _messages;
    private string _currentMessageText = string.Empty;
    private string _searchText = string.Empty;
    private Chat _selectedChat;

    private User _currentUser;

    public ChatViewModel(IChatsApiClient chatsApiClient, IMessagesApiClient messagesApiClient, IMainThread mainThread, IChatRealtimeService chatRealtimeService)
    {
        _chatsApiClient = chatsApiClient;
        _messagesApiClient = messagesApiClient;
        _mainThread = mainThread;
        _chatRealtimeService = chatRealtimeService;

        _currentUser = new User()
        {
            Username = "Me"
        };

        _messages = new ObservableCollection<Message>();
        FilteredMessages = new ObservableCollection<Message>();
        ChatsList = new ObservableCollection<Chat>();

        _messages.CollectionChanged += MessagesCollectionChanged;
        _chatRealtimeService.OnMessageReceived += OnNewMessageReceived;

        SubmitCommand = new AsyncCommand(SubmitClicked, CheckSubmitCanExecute);
        CleanupCommand = new AsyncCommand(CleanupClicked, CheckCleanupCanExecute);
    }

    private void OnNewMessageReceived(Message newMessage)
    {
        _messages.Add(newMessage);
    }

    public ObservableCollection<Message> FilteredMessages { get; private set; }

    public ObservableCollection<Chat> ChatsList { get; private set; }

    public Chat SelectedChat
    {
        get => _selectedChat;
        set
        {
            var isChanged = SetProperty(ref _selectedChat, value);
            if (!isChanged) return;
            _ = LoadMessagesAsync(_selectedChat, CancellationToken.None);
        }
    }

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

    public override async Task OnAppearing()
    {
        await base.OnAppearing();

        await _chatRealtimeService.ConnectAsync();
        var chats = await _chatsApiClient.GetAllAsync();

        _mainThread.BeginInvokeOnMainThreadAsync(() =>
        {
            ChatsList = new ObservableCollection<Chat>(chats);
            RaisePropertyChangedEvent(nameof(ChatsList));
        });
    }

    private async Task LoadMessagesAsync(Chat chat, CancellationToken cancellationToken)
    {
        var messages = await _messagesApiClient.GetAllAsync(chat.Id);

        _mainThread.BeginInvokeOnMainThreadAsync(() =>
        {
            _messages.CollectionChanged -= MessagesCollectionChanged;
            _messages = new ObservableCollection<Message>(messages ?? []);
            _messages.CollectionChanged += MessagesCollectionChanged;
            FilteredMessages = new(messages ?? []);
            RaisePropertyChangedEvent(nameof(FilteredMessages));
            RaisePropertyChangedEvent(nameof(IsSearchEnabled));
        });
    }

    private async Task SubmitClicked()
    {
        if (string.IsNullOrWhiteSpace(_currentMessageText))
        {
            return;
        }

        var message = await _messagesApiClient.SendMessage(new DataContracts.MessageCreateDto()
        {
            ChatId = _selectedChat.Id,
            Text = _currentMessageText,
        });

        //if (message != null)
        //{
        //    _mainThread.BeginInvokeOnMainThreadAsync(() =>
        //    {
        //        _messages.Add(message);
        //    });
        //}

        CurrentMessageText = string.Empty;
        CleanupCommand.RaiseCanExecuteChanged();
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
