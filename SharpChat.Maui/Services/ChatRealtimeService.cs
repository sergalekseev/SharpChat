using Microsoft.AspNetCore.SignalR.Client;
using SharpChat.Core.Models;
using SharpChat.Core.Services;

namespace SharpChat.Maui.Services;

public class ChatRealtimeService : IChatRealtimeService
{
    private HubConnection _connection;
    private readonly IAuthService _authService;

    public ChatRealtimeService(IAuthService authService)
    {
        _authService = authService;
    }

    public event Action<Message>? OnMessageReceived;

    public async Task ConnectAsync()
    {
        if (_connection != null &&
            _connection.State == HubConnectionState.Connected)
            return;

        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5153/chatnotifications", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(_authService.AuthToken ?? null);
            })
            .WithAutomaticReconnect()
            .Build();

        RegisterHandlers();

        await _connection.StartAsync();
    }

    private void RegisterHandlers()
    {
        _connection.On<Message>(nameof(ReceiveNewMessage), ReceiveNewMessage);
    }

    public async Task DisconnectAsync()
    {
        if (_connection != null)
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            _connection = null;
        }
    }

    public Task ReceiveNewMessage(Message message)
    {
        OnMessageReceived?.Invoke(message);
        return Task.CompletedTask;
    }
}
