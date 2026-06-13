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

    private Dictionary<int, ObservableCollection<Message>> _messagesCache = new Dictionary<int, ObservableCollection<Message>>();

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

        FilteredMessages = new ObservableCollection<Message>();
        ChatsList = new ObservableCollection<Chat>();

        _chatRealtimeService.OnMessageReceived += OnNewMessageReceived;

        SubmitCommand = new AsyncCommand(SubmitClicked, CheckSubmitCanExecute);
        CleanupCommand = new AsyncCommand(CleanupClicked, CheckCleanupCanExecute);
    }

    private void OnNewMessageReceived(Message newMessage)
    {
        _messagesCache.TryGetValue(newMessage.ChatId, out var messages);

        if (messages == null)
        {
            messages = new ObservableCollection<Message>();
            _messagesCache.Add(newMessage.ChatId, messages);
        }

        messages.Add(newMessage);
    }

    private ObservableCollection<Message>? CurrentChatMessages
    {
        get
        {
            if (SelectedChat == null)
            {
                return null;
            }

            if (_messagesCache.ContainsKey(SelectedChat.Id))
            {
                return _messagesCache[SelectedChat.Id];
            }

            return null;
        }
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

            var messages = CurrentChatMessages;
            if (messages == null)
            {
                _ = LoadMessagesAsync(_selectedChat, CancellationToken.None);
            }
            else
            {
                UpdateMessagesView();
            }
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

    public bool IsSearchEnabled => CurrentChatMessages?.Count > 0;

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
        var currentMessages = CurrentChatMessages;
        if (currentMessages != null) return;

        var messages = await _messagesApiClient.GetAllAsync(chat.Id);
        currentMessages = new ObservableCollection<Message>(messages ?? []);
        _messagesCache.Add(chat.Id, currentMessages);
        UpdateMessagesView();
    }

    private void UpdateMessagesView()
    {
        _mainThread.BeginInvokeOnMainThreadAsync(() =>
        {
            var currentMessages = CurrentChatMessages;

            if (currentMessages == null) return;


            currentMessages.CollectionChanged -= MessagesCollectionChanged;
            currentMessages.CollectionChanged += MessagesCollectionChanged;

            FilteredMessages = new(currentMessages ?? []);
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
        CurrentChatMessages?.Clear();
        CleanupCommand.RaiseCanExecuteChanged();

        return Task.CompletedTask;
    }

    private bool CheckCleanupCanExecute()
    {
        return CurrentChatMessages?.Count > 0;
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

        var targetList = CurrentChatMessages?.Where(message =>
            isSearchTextNotValuable || message.Text.Contains(SearchText)).ToList();

        for (var i = FilteredMessages.Count - 1; i >= 0; i--)
        {
            if (isSearchTextNotValuable)
            {
                break;
            }

            var item = FilteredMessages[i];
            if (targetList?.Contains(item) == false)
            {
                FilteredMessages.RemoveAt(i);
            }
        }

        for (var i = 0; i < targetList?.Count; i++)
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
