using SharpChat.Core.Models;

namespace SharpChat.Core.Services;

public interface IChatNotificationsClient
{
    Task ReceiveNewMessage(Message message);
}

