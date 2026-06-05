using SharpChat.Core.Models;

namespace SharpChat.Server.Services;

public static class MockedDataGenerator
{

    public static readonly List<Chat> Chats =
        [
            new Chat(){ Id = 0, Title = "Test1" },
            new Chat(){ Id = 1, Title = "Test2" },
            new Chat(){ Id = 2, Title = "Test3" },
        ];

    public static List<Message>? GenerateMessagesForChat(Chat chat)
    {
        var user1 = new User() { Username = chat.Title };
        var user2 = new User() { Username = "Me" };
        var messageTime = DateTime.Now;

        return chat.Id switch
        {
            0 => new List<Message>() {
                new Message() { Id = 0, ChatId = chat.Id, Sender = user2, Text = $"Hi {user1.Username}", Time = messageTime},
                new Message() { Id = 1, ChatId = chat.Id, Sender = user1, Text = $"Hello, how are you?", Time = messageTime.AddMinutes(1)},
                new Message() { Id = 2, ChatId = chat.Id, Sender = user2, Text = $"Good, thanks", Time = messageTime.AddMinutes(2)},
            },
            1 => new List<Message>() {
                new Message() { Id = 0, ChatId = chat.Id, Sender = user2, Text = $"Hello {user1.Username}", Time = messageTime},
                new Message() { Id = 1, ChatId = chat.Id, Sender = user2, Text = $"Do you have time to call?", Time = messageTime.AddMinutes(1)},
                new Message() { Id = 2, ChatId = chat.Id, Sender = user1, Text = $"Not now, I'll ping you later", Time = messageTime.AddMinutes(2)},
            },
            _ => null
        };
    }
}
