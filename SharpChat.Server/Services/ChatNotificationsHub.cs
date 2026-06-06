using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SharpChat.Core.Services;

namespace SharpChat.Server.Services;

[Authorize]
public class ChatNotificationsHub : Hub<IChatNotificationsClient>
{
    private readonly IChatsManager _chatsManager;

    public ChatNotificationsHub(IChatsManager chatsManager)
    {
        _chatsManager = chatsManager;
    }

    public override async Task OnConnectedAsync()
    {
        var user = Context.User;

        if (user == null)
        {
            Context.Abort();
            return;
        }

        var chats = _chatsManager.GetUserChats(user);

        foreach (var chat in chats)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"chat_{chat.Id}");
        }

        await base.OnConnectedAsync();
    }


}

