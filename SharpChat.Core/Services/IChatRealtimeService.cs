using SharpChat.Core.Models;

namespace SharpChat.Core.Services;

public interface IChatRealtimeService : IChatNotificationsClient
{
    Task ConnectAsync();
    Task DisconnectAsync();

    event Action<Message> OnMessageReceived;
}

