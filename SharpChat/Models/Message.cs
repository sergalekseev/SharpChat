namespace SharpChat.Core.Models
{
    public class Message
    {
        public User Sender { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }
}
