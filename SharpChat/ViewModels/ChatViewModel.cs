using SharpChat.Core.Models;
using System.Collections.ObjectModel;

namespace SharpChat.Core.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        public ObservableCollection<Message> Messages { get; }

        public ChatViewModel()
        {
            var user1 = new User()
            {
                Username = "User1"
            };

            var user2 = new User()
            {
                Username = "User2"
            };

            Messages = new()
            {
                new() { Sender = user1, Text = "Hello" },
                new() { Sender = user2, Text = "World" },
            };
        }
    }
}
