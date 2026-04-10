namespace SharpChat.Core.Models;

internal class Chat
{
    public int Id { get; set; }
    public string Title { get; set; }
    public Message LastMessage { get; set; }
}

