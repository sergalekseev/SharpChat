using SharpChat.Core.Models;

namespace SharpChat.Core.Services;

public interface IChatRealtimeService
{
    Task ConnectAsync();
    Task DisconnectAsync();

    event Action<Message> OnMessageReceived;
}

