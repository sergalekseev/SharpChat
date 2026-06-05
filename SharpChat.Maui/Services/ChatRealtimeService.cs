using Microsoft.AspNetCore.SignalR.Client;
using SharpChat.Core;
using SharpChat.Core.Models;
using SharpChat.Core.Services;

namespace SharpChat.Maui.Services;

public class ChatRealtimeService : IChatRealtimeService
{
    private HubConnection _connection;

    public event Action<Message>? OnMessageReceived;

    public async Task ConnectAsync()
    {
        if (_connection != null &&
            _connection.State == HubConnectionState.Connected)
            return;

        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5153/chatnotifications", options =>
            {
                options.AccessTokenProvider = async () =>
                    await SecureStorage.GetAsync(Consts.StorageAccessTokenKey);
            })
            .WithAutomaticReconnect()
            .Build();

        RegisterHandlers();

        await _connection.StartAsync();
    }

    private void RegisterHandlers()
    {
        _connection.On<Message>("ReceiveNewMessage", message =>
        {
            OnMessageReceived?.Invoke(message);
        });
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
}
